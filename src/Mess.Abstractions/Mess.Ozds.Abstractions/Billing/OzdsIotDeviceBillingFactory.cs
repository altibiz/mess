using Mess.Billing.Abstractions.Services;
using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql;
using ISession = YesSql.ISession;

namespace Mess.Ozds.Abstractions.Billing;

// TODO: shorten the hell out of this thing

public abstract class OzdsClosedDistributionUnitBillingFactory<T> : IBillingFactory
  where T : ContentItemBase
{
  private readonly IContentManager _contnetManager;

  private readonly ISession _session;

  protected OzdsClosedDistributionUnitBillingFactory(IContentManager contentManager, ISession session)
  {
    _contnetManager = contentManager;
    _session = session;
  }

  public ContentItem CreateInvoice(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var measurementDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new InvalidOperationException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new InvalidOperationException("OzdsIotDevicePart is null");

    var ozdsIotDeviceIndex =
      _session
        .QueryIndex<OzdsIotDeviceIndex>()
        .Where(
          index => index.OzdsIotDeviceContentItemId == contentItem.ContentItemId
        )
        .FirstOrDefaultAsync()
        .Result
      ?? throw new InvalidOperationException("OzdsIotDeviceIndex is null");

    var unitItem =
      _contnetManager
        .GetContentAsync<DistributionSystemUnitItem>(
          ozdsIotDeviceIndex.DistributionSystemUnitContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        "Distribution system unit not found"
      );
    var systemItem =
      _contnetManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          ozdsIotDeviceIndex.ClosedDistributionSystemContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        "Closed distribution system not found"
      );
    var operatorItem =
      _contnetManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          ozdsIotDeviceIndex.DistributionSystemOperatorContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      _contnetManager
        .GetContentAsync<RegulatoryAgencyCatalogueItem>(
          operatorItem.DistributionSystemOperatorPart.Value
            .RegulatoryAgencyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException("Usage catalogue not found");
    var supplyCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException("Supply catalogue not found");

    var billingData =
      FetchBillingData(contentItem.AsContent<T>(), fromDate, toDate)
      ?? throw new InvalidOperationException("Billing data is null");

    var calculationData = CreateCalculationData(
      fromDate,
      toDate,
      billingData,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    var invoiceData = CreateInvoiceData(
      calculationData,
      regulatoryAgencyCatalogueItem
    );

    var invoiceItem = _contnetManager.NewContentAsync<OzdsInvoiceItem>().Result;
    invoiceItem.Alter(
      invoiceContentItem => invoiceContentItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.IotDevice = contentItem;
        ozdsCalculationPart.RegulatoryAgencyCatalogue =
          regulatoryAgencyCatalogueItem;
        ozdsCalculationPart.UsageCatalogue = usageCatalogueItem;
        ozdsCalculationPart.SupplyCatalogue = supplyCatalogueItem;
        ozdsCalculationPart.Data = calculationData;
      }
    );
    invoiceItem.Alter(
      invoiceContentItem => invoiceContentItem.OzdsInvoicePart,
      ozdsInvoicePart =>
      {
        ozdsInvoicePart.DistributionSystemOperator = operatorItem;
        ozdsInvoicePart.ClosedDistributionSystem = systemItem;
        ozdsInvoicePart.DistributionSystemUnit = unitItem;
        ozdsInvoicePart.Data = invoiceData;
      }
    );

    return invoiceItem;
  }

  public async Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var measurementDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new InvalidOperationException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new InvalidOperationException("OzdsIotDevicePart is null");

    var ozdsIotDeviceIndex =
      await _session
        .QueryIndex<OzdsIotDeviceIndex>()
        .Where(
          index => index.OzdsIotDeviceContentItemId == contentItem.ContentItemId
        )
        .FirstOrDefaultAsync()
      ?? throw new InvalidOperationException("OzdsIotDeviceIndex is null");

    var unitItem =
      await _contnetManager.GetContentAsync<DistributionSystemUnitItem>(
        ozdsIotDeviceIndex.DistributionSystemUnitContentItemId
      )
      ?? throw new InvalidOperationException(
        "Distribution system unit not found"
      );
    var systemItem =
      await _contnetManager.GetContentAsync<ClosedDistributionSystemItem>(
        ozdsIotDeviceIndex.ClosedDistributionSystemContentItemId
      )
      ?? throw new InvalidOperationException(
        "Closed distribution system not found"
      );
    var operatorItem =
      await _contnetManager.GetContentAsync<DistributionSystemOperatorItem>(
        ozdsIotDeviceIndex.DistributionSystemOperatorContentItemId
      )
      ?? throw new InvalidOperationException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      await _contnetManager.GetContentAsync<RegulatoryAgencyCatalogueItem>(
        operatorItem.DistributionSystemOperatorPart.Value
          .RegulatoryAgencyCatalogue.ContentItemIds.First()
      )
      ?? throw new InvalidOperationException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
      ) ?? throw new InvalidOperationException("Usage catalogue not found");
    var supplyCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
      ) ?? throw new InvalidOperationException("Supply catalogue not found");

    var billingData =
      await FetchBillingDataAsync(contentItem.AsContent<T>(), fromDate, toDate)
      ?? throw new InvalidOperationException("Billing data is null");

    var calculationData = await CreateCalculationDataAsync(
      fromDate,
      toDate,
      billingData,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    var invoiceData = await CreateInvoiceDataAsync(
      calculationData,
      regulatoryAgencyCatalogueItem
    );

    var invoiceItem = await _contnetManager.NewContentAsync<OzdsInvoiceItem>();
    invoiceItem.Alter(
      invoiceContentItem => invoiceContentItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.IotDevice = contentItem;
        ozdsCalculationPart.RegulatoryAgencyCatalogue =
          regulatoryAgencyCatalogueItem;
        ozdsCalculationPart.UsageCatalogue = usageCatalogueItem;
        ozdsCalculationPart.SupplyCatalogue = supplyCatalogueItem;
        ozdsCalculationPart.Data = calculationData;
      }
    );
    invoiceItem.Alter(
      invoiceContentItem => invoiceContentItem.OzdsInvoicePart,
      ozdsInvoicePart =>
      {
        ozdsInvoicePart.DistributionSystemOperator = operatorItem;
        ozdsInvoicePart.ClosedDistributionSystem = systemItem;
        ozdsInvoicePart.DistributionSystemUnit = unitItem;
        ozdsInvoicePart.Data = invoiceData;
      }
    );

    return invoiceItem;
  }

  public ContentItem CreateReceipt(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
    var measurementDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new InvalidOperationException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new InvalidOperationException("OzdsIotDevicePart is null");

    var invoiceItem =
      invoiceContentItem.AsContent<OzdsInvoiceItem>()
      ?? throw new InvalidOperationException("InvoiceItem is null");

    var ozdsIotDeviceIndex =
      _session
        .QueryIndex<OzdsIotDeviceIndex>()
        .Where(
          index => index.OzdsIotDeviceContentItemId == contentItem.ContentItemId
        )
        .FirstOrDefaultAsync()
        .Result
      ?? throw new InvalidOperationException("OzdsIotDeviceIndex is null");

    var unitItem =
      _contnetManager
        .GetContentAsync<DistributionSystemUnitItem>(
          ozdsIotDeviceIndex.DistributionSystemUnitContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        "Distribution system unit not found"
      );
    var systemItem =
      _contnetManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          ozdsIotDeviceIndex.ClosedDistributionSystemContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        "Closed distribution system not found"
      );
    var operatorItem =
      _contnetManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          ozdsIotDeviceIndex.DistributionSystemOperatorContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      _contnetManager
        .GetContentAsync<RegulatoryAgencyCatalogueItem>(
          operatorItem.DistributionSystemOperatorPart.Value
            .RegulatoryAgencyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException("Usage catalogue not found");
    var supplyCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException("Supply catalogue not found");

    var receiptData = CreateReceiptData(invoiceItem.OzdsInvoicePart.Value.Data);

    var receiptItem = _contnetManager.NewContentAsync<OzdsReceiptItem>().Result;
    receiptItem.Alter(
      receiptItem => receiptItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.IotDevice = contentItem;
        ozdsCalculationPart.RegulatoryAgencyCatalogue =
          regulatoryAgencyCatalogueItem;
        ozdsCalculationPart.UsageCatalogue = usageCatalogueItem;
        ozdsCalculationPart.SupplyCatalogue = supplyCatalogueItem;
        ozdsCalculationPart.Data = invoiceItem.OzdsCalculationPart.Value.Data;
      }
    );
    receiptItem.Alter(
      receiptItem => receiptItem.OzdsReceiptPart,
      ozdsReceiptPart =>
      {
        ozdsReceiptPart.DistributionSystemOperator = operatorItem;
        ozdsReceiptPart.ClosedDistributionSystem = systemItem;
        ozdsReceiptPart.DistributionSystemUnit = unitItem;
        ozdsReceiptPart.Data = receiptData;
      }
    );

    return receiptItem;
  }

  public async Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
    var iotDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new InvalidOperationException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new InvalidOperationException("OzdsIotDevicePart is null");

    var invoiceItem =
      invoiceContentItem.AsContent<OzdsInvoiceItem>()
      ?? throw new InvalidOperationException("InvoiceItem is null");

    var ozdsIotDeviceIndex =
      _session
        .QueryIndex<OzdsIotDeviceIndex>()
        .Where(
          index => index.OzdsIotDeviceContentItemId == contentItem.ContentItemId
        )
        .FirstOrDefaultAsync()
        .Result
      ?? throw new InvalidOperationException("OzdsIotDeviceIndex is null");

    var unitItem =
      await _contnetManager.GetContentAsync<DistributionSystemUnitItem>(
        ozdsIotDeviceIndex.DistributionSystemUnitContentItemId
      )
      ?? throw new InvalidOperationException(
        "Distribution system unit not found"
      );
    var systemItem =
      await _contnetManager.GetContentAsync<ClosedDistributionSystemItem>(
        ozdsIotDeviceIndex.ClosedDistributionSystemContentItemId
      )
      ?? throw new InvalidOperationException(
        "Closed distribution system not found"
      );
    var operatorItem =
      await _contnetManager.GetContentAsync<DistributionSystemOperatorItem>(
        ozdsIotDeviceIndex.DistributionSystemOperatorContentItemId
      )
      ?? throw new InvalidOperationException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      await _contnetManager.GetContentAsync<RegulatoryAgencyCatalogueItem>(
        operatorItem.DistributionSystemOperatorPart.Value
          .RegulatoryAgencyCatalogue.ContentItemIds.First()
      )
      ?? throw new InvalidOperationException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
      ) ?? throw new InvalidOperationException("Usage catalogue not found");
    var supplyCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
      ) ?? throw new InvalidOperationException("Supply catalogue not found");

    var receiptData = await CreateReceiptDataAsync(
      invoiceItem.OzdsInvoicePart.Value.Data
    );

    var receiptItem = await _contnetManager.NewContentAsync<OzdsReceiptItem>();
    receiptItem.Alter(
      receiptItem => receiptItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.IotDevice = contentItem;
        ozdsCalculationPart.RegulatoryAgencyCatalogue =
          regulatoryAgencyCatalogueItem;
        ozdsCalculationPart.UsageCatalogue = usageCatalogueItem;
        ozdsCalculationPart.SupplyCatalogue = supplyCatalogueItem;
        ozdsCalculationPart.Data = invoiceItem.OzdsCalculationPart.Value.Data;
      }
    );
    receiptItem.Alter(
      receiptItem => receiptItem.OzdsReceiptPart,
      ozdsReceiptPart =>
      {
        ozdsReceiptPart.DistributionSystemOperator = operatorItem;
        ozdsReceiptPart.ClosedDistributionSystem = systemItem;
        ozdsReceiptPart.DistributionSystemUnit = unitItem;
        ozdsReceiptPart.Data = receiptData;
      }
    );

    return receiptItem;
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == typeof(T).ContentTypeName();
  }

  protected abstract OzdsIotDeviceBillingData? FetchBillingData(
    T measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  protected abstract Task<OzdsIotDeviceBillingData?> FetchBillingDataAsync(
    T measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  private static OzdsCalculationData CreateCalculationData(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    OzdsIotDeviceBillingData billingData,
    OperatorCatalogueItem usageCatalogueItem,
    OperatorCatalogueItem supplyCatalogueItem
  )
  {
    var usageExpenditure = CreateExpenditureData(
      billingData,
      usageCatalogueItem
    );
    var supplyExpenditure = CreateExpenditureData(
      billingData,
      supplyCatalogueItem
    );

    return new OzdsCalculationData(
      fromDate,
      toDate,
      usageExpenditure,
      supplyExpenditure
    );
  }

  private static OzdsExpenditureData CreateExpenditureData(
    OzdsIotDeviceBillingData billingData,
    OperatorCatalogueItem catalogueItem
  )
  {
    var highTariffEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M;
    var highTariffEnergyAmount =
      billingData.EndHighTariffEnergy_kWh
      - billingData.StartHighTariffEnergy_kWh;
    var highTariffEnergyTotal = highTariffEnergyAmount * highTariffEnergyPrice;
    var highTariffEnergyItem =
      highTariffEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartHighTariffEnergy_kWh,
          billingData.EndHighTariffEnergy_kWh,
          highTariffEnergyAmount,
          highTariffEnergyPrice,
          highTariffEnergyTotal
        );

    var lowTariffEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M;
    var lowTariffEnergyAmount =
      billingData.EndLowTariffEnergy_kWh - billingData.StartLowTariffEnergy_kWh;
    var lowTariffEnergyTotal = lowTariffEnergyAmount * lowTariffEnergyPrice;
    var lowTariffEnergyItem =
      lowTariffEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartLowTariffEnergy_kWh,
          billingData.EndLowTariffEnergy_kWh,
          lowTariffEnergyAmount,
          lowTariffEnergyPrice,
          lowTariffEnergyTotal
        );

    var energyPrice =
      catalogueItem.OperatorCataloguePart.Value.EnergyPrice.Value ?? 0.0M;
    var energyAmount =
      billingData.EndEnergyTotal_kWh - billingData.StartEnergyTotal_kWh;
    var energyTotal = energyAmount * energyPrice;
    var energyItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartEnergyTotal_kWh,
          billingData.EndEnergyTotal_kWh,
          energyAmount,
          energyPrice,
          energyTotal
        );

    var maxPowerPrice =
      catalogueItem.OperatorCataloguePart.Value.MaxPowerPrice.Value ?? 0.0M;
    var maxPowerAmount = billingData.PeakPowerTotal_kW;
    var maxPowerTotal = maxPowerAmount * maxPowerPrice;
    var maxPowerItem =
      maxPowerPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          0.0M,
          0.0M,
          maxPowerAmount,
          maxPowerPrice,
          maxPowerTotal
        );

    var iotDeviceFeePrice =
      catalogueItem.OperatorCataloguePart.Value.IotDeviceFee.Value ?? 0.0M;
    var iotDeviceFeeAmount = 1.0M;
    var iotDeviceFeeTotal = iotDeviceFeeAmount * iotDeviceFeePrice;
    var iotDeviceFee =
      iotDeviceFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          0.0M,
          0.0M,
          iotDeviceFeeAmount,
          iotDeviceFeePrice,
          iotDeviceFeeTotal
        );

    return new OzdsExpenditureData(
      highTariffEnergyItem,
      lowTariffEnergyItem,
      energyItem,
      maxPowerItem,
      iotDeviceFee,
      (highTariffEnergyItem?.Total ?? 0.0M)
      + (lowTariffEnergyItem?.Total ?? 0.0M)
      + (energyItem?.Total ?? 0.0M)
      + (maxPowerItem?.Total ?? 0.0M)
      + (iotDeviceFee?.Total ?? 0.0M)
    );
  }

  private static async Task<OzdsCalculationData> CreateCalculationDataAsync(
    DateTimeOffset from,
    DateTimeOffset to,
    OzdsIotDeviceBillingData billingData,
    OperatorCatalogueItem usageCatalogueItem,
    OperatorCatalogueItem supplyCatalogueItem
  )
  {
    return CreateCalculationData(
      from,
      to,
      billingData,
      usageCatalogueItem,
      supplyCatalogueItem
    );
  }

  private static OzdsInvoiceData CreateInvoiceData(
    OzdsCalculationData calculationData,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem
  )
  {
    var usageFee = calculationData.UsageExpenditure.Total;

    var supplyFee = calculationData.SupplyExpenditure.Total;

    var renewableEnergyFeePrice =
      regulatoryAgencyCatalogueItem
        .RegulatoryAgencyCataloguePart
        .Value
        .RenewableEnergyFee
        .Value ?? 0.0M;
    var renewableEnergyFeeAmount =
      calculationData.SupplyExpenditure.EnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.LowEnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.HighEnergyItem?.Amount
      ?? 0.0M;
    var renewableEnergyFeeItem =
      renewableEnergyFeePrice == 0.0M
        ? null
        : new OzdsInvoiceFeeData(
          renewableEnergyFeeAmount,
          renewableEnergyFeePrice,
          renewableEnergyFeeAmount * renewableEnergyFeePrice
        );

    var businessUsageFeePrice =
      regulatoryAgencyCatalogueItem
        .RegulatoryAgencyCataloguePart
        .Value
        .BusinessUsageFee
        .Value ?? 0.0M;
    var businessUsageFeeAmount =
      calculationData.SupplyExpenditure.EnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.LowEnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.HighEnergyItem?.Amount
      ?? 0.0M;
    var businessUsageFeeItem =
      businessUsageFeePrice == 0.0M
        ? null
        : new OzdsInvoiceFeeData(
          businessUsageFeeAmount,
          businessUsageFeePrice,
          businessUsageFeeAmount * businessUsageFeePrice
        );

    var total =
      usageFee + supplyFee + renewableEnergyFeeItem?.Total
      ?? 0.0M + businessUsageFeeItem?.Total
      ?? 0.0M;
    var taxRate =
      regulatoryAgencyCatalogueItem
        .RegulatoryAgencyCataloguePart
        .Value
        ?.TaxRate
        .Value ?? 0.0M;
    var tax = total * taxRate;
    var totalWithTax = total + tax;

    return new OzdsInvoiceData(
      calculationData.From,
      calculationData.To,
      usageFee,
      supplyFee,
      renewableEnergyFeeItem,
      businessUsageFeeItem,
      total,
      taxRate,
      tax,
      totalWithTax
    );
  }

  private static async Task<OzdsInvoiceData> CreateInvoiceDataAsync(
    OzdsCalculationData calculationData,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem
  )
  {
    return CreateInvoiceData(calculationData, regulatoryAgencyCatalogueItem);
  }

  private static OzdsReceiptData CreateReceiptData(OzdsInvoiceData invoiceData)
  {
    var renewableEnergyFee =
      invoiceData.RenewableEnergyFee == null
        ? null
        : new OzdsReceiptFeeData(
          invoiceData.RenewableEnergyFee.Amount,
          invoiceData.RenewableEnergyFee.UnitPrice,
          invoiceData.RenewableEnergyFee.Total
        );

    var businessUsageFee =
      invoiceData.BusinessUsageFee == null
        ? null
        : new OzdsReceiptFeeData(
          invoiceData.BusinessUsageFee.Amount,
          invoiceData.BusinessUsageFee.UnitPrice,
          invoiceData.BusinessUsageFee.Total
        );

    return new OzdsReceiptData(
      invoiceData.From,
      invoiceData.To,
      invoiceData.UsageFee,
      invoiceData.SupplyFee,
      renewableEnergyFee,
      businessUsageFee,
      invoiceData.Total,
      invoiceData.TaxRate,
      invoiceData.Tax,
      invoiceData.TotalWithTax
    );
  }

  private static async Task<OzdsReceiptData> CreateReceiptDataAsync(
    OzdsInvoiceData invoiceData
  )
  {
    return CreateReceiptData(invoiceData);
  }
}
