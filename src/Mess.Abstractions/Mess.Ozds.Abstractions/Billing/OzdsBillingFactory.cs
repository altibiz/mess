using Mess.Billing.Abstractions.Services;
using Mess.Iot.Abstractions.Models;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using ISession = YesSql.ISession;

namespace Mess.Ozds.Abstractions.Billing;

public abstract class OzdsBillingFactory<T> : IBillingFactory
  where T : ContentItemBase
{
  protected abstract OzdsBillingData? FetchBillingData(
    T measurementDeviceItem,
    DateTimeOffset from,
    DateTimeOffset to
  );

  protected abstract Task<OzdsBillingData?> FetchBillingDataAsync(
    T measurementDeviceItem,
    DateTimeOffset from,
    DateTimeOffset to
  );

  public string ContentType => typeof(T).ContentTypeName();

  public ContentItem CreateInvoice(
    ContentItem contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    var measurementDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new NullReferenceException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new NullReferenceException("OzdsIotDevicePart is null");

    var unitItem =
      _contnetManager
        .GetContentAsync<DistributionSystemUnitItem>(
          ozdsIotDevicePart.DistributionSystemUnitContentItemId
        )
        .Result
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      _contnetManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          ozdsIotDevicePart.ClosedDistributionSystemContentItemId
        )
        .Result
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      _contnetManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          ozdsIotDevicePart.DistributionSystemOperatorContentItemId
        )
        .Result
      ?? throw new NullReferenceException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      _contnetManager
        .GetContentAsync<RegulatoryAgencyCatalogueItem>(
          operatorItem.DistributionSystemOperatorPart.Value.RegulatoryCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Supply catalogue not found");

    var billingData =
      FetchBillingData(contentItem.AsContent<T>(), from, to)
      ?? throw new NullReferenceException("Billing data is null");

    var calculationData = CreateCalculationData(
      from,
      to,
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
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    var measurementDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new NullReferenceException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new NullReferenceException("OzdsIotDevicePart is null");

    var unitItem =
      await _contnetManager.GetContentAsync<DistributionSystemUnitItem>(
        ozdsIotDevicePart.DistributionSystemUnitContentItemId
      )
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      await _contnetManager.GetContentAsync<ClosedDistributionSystemItem>(
        ozdsIotDevicePart.ClosedDistributionSystemContentItemId
      )
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      await _contnetManager.GetContentAsync<DistributionSystemOperatorItem>(
        ozdsIotDevicePart.DistributionSystemOperatorContentItemId
      )
      ?? throw new NullReferenceException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      await _contnetManager.GetContentAsync<RegulatoryAgencyCatalogueItem>(
        operatorItem.DistributionSystemOperatorPart.Value.RegulatoryCatalogue.ContentItemIds.First()
      )
      ?? throw new NullReferenceException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Supply catalogue not found");

    var billingData =
      await FetchBillingDataAsync(contentItem.AsContent<T>(), from, to)
      ?? throw new NullReferenceException("Billing data is null");

    var calculationData = await CreateCalculationDataAsync(
      from,
      to,
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
      ?? throw new NullReferenceException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new NullReferenceException("OzdsIotDevicePart is null");

    var invoiceItem =
      invoiceContentItem.AsContent<OzdsInvoiceItem>()
      ?? throw new NullReferenceException("InvoiceItem is null");

    var unitItem =
      _contnetManager
        .GetContentAsync<DistributionSystemUnitItem>(
          ozdsIotDevicePart.DistributionSystemUnitContentItemId
        )
        .Result
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      _contnetManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          ozdsIotDevicePart.ClosedDistributionSystemContentItemId
        )
        .Result
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      _contnetManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          ozdsIotDevicePart.DistributionSystemOperatorContentItemId
        )
        .Result
      ?? throw new NullReferenceException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      _contnetManager
        .GetContentAsync<RegulatoryAgencyCatalogueItem>(
          operatorItem.DistributionSystemOperatorPart.Value.RegulatoryCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Supply catalogue not found");

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
    var measurementDevicePart =
      contentItem.As<IotDevicePart>()
      ?? throw new NullReferenceException("IotDevicePart is null");
    var ozdsIotDevicePart =
      contentItem.As<OzdsIotDevicePart>()
      ?? throw new NullReferenceException("OzdsIotDevicePart is null");

    var invoiceItem =
      invoiceContentItem.AsContent<OzdsInvoiceItem>()
      ?? throw new NullReferenceException("InvoiceItem is null");

    var unitItem =
      await _contnetManager.GetContentAsync<DistributionSystemUnitItem>(
        ozdsIotDevicePart.DistributionSystemUnitContentItemId
      )
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      await _contnetManager.GetContentAsync<ClosedDistributionSystemItem>(
        ozdsIotDevicePart.ClosedDistributionSystemContentItemId
      )
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      await _contnetManager.GetContentAsync<DistributionSystemOperatorItem>(
        ozdsIotDevicePart.DistributionSystemOperatorContentItemId
      )
      ?? throw new NullReferenceException(
        "Distribution system operator not found"
      );

    var regulatoryAgencyCatalogueItem =
      await _contnetManager.GetContentAsync<RegulatoryAgencyCatalogueItem>(
        operatorItem.DistributionSystemOperatorPart.Value.RegulatoryCatalogue.ContentItemIds.First()
      )
      ?? throw new NullReferenceException(
        "Regulatory agency catalogue not found"
      );
    var usageCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Supply catalogue not found");

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

  private OzdsCalculationData CreateCalculationData(
    DateTimeOffset from,
    DateTimeOffset to,
    OzdsBillingData billingData,
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
      From: from,
      To: to,
      UsageExpenditure: usageExpenditure,
      SupplyExpenditure: supplyExpenditure
    );
  }

  private OzdsExpenditureData CreateExpenditureData(
    OzdsBillingData billingData,
    OperatorCatalogueItem catalogueItem
  )
  {
    var highEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M;
    var highEnergyAmount = billingData.EndEnergy - billingData.StartEnergy;
    var highEnergyTotal = highEnergyAmount * highEnergyPrice;
    var highEnergyItem =
      highEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          ValueFrom: billingData.StartEnergy,
          ValueTo: billingData.EndEnergy,
          Amount: highEnergyAmount,
          UnitPrice: highEnergyPrice,
          Total: highEnergyTotal
        );

    var lowEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M;
    var lowEnergyAmount = billingData.EndLowEnergy - billingData.StartLowEnergy;
    var lowEnergyTotal = lowEnergyAmount * lowEnergyPrice;
    var lowEnergyItem =
      lowEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          ValueFrom: billingData.StartLowEnergy,
          ValueTo: billingData.EndLowEnergy,
          Amount: lowEnergyAmount,
          UnitPrice: lowEnergyPrice,
          Total: lowEnergyTotal
        );

    var energyPrice =
      catalogueItem.OperatorCataloguePart.Value.EnergyPrice.Value ?? 0.0M;
    var energyAmount = billingData.EndEnergy - billingData.StartEnergy;
    var energyTotal = energyAmount * energyPrice;
    var energyItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          ValueFrom: billingData.StartEnergy,
          ValueTo: billingData.EndEnergy,
          Amount: energyAmount,
          UnitPrice: energyPrice,
          Total: energyTotal
        );

    var maxPowerPrice =
      catalogueItem.OperatorCataloguePart.Value.MaxPowerPrice.Value ?? 0.0M;
    var maxPowerAmount = billingData.MaxPower;
    var maxPowerTotal = maxPowerAmount * maxPowerPrice;
    var maxPowerItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          ValueFrom: 0.0M,
          ValueTo: 0.0M,
          Amount: maxPowerAmount,
          UnitPrice: maxPowerPrice,
          Total: maxPowerTotal
        );

    var measurementDeviceFeePrice =
      catalogueItem.OperatorCataloguePart.Value.IotDeviceFee.Value
      ?? 0.0M;
    var measurementDeviceFeeAmount = 1.0M;
    var measurementDeviceFeeTotal =
      measurementDeviceFeeAmount * measurementDeviceFeePrice;
    var measurementDeviceFeeItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          ValueFrom: 0.0M,
          ValueTo: 0.0M,
          Amount: measurementDeviceFeeAmount,
          UnitPrice: measurementDeviceFeePrice,
          Total: measurementDeviceFeeTotal
        );

    return new OzdsExpenditureData(
      HighEnergyItem: highEnergyItem,
      LowEnergyItem: lowEnergyItem,
      EnergyItem: energyItem,
      MaxPowerItem: maxPowerItem,
      IotDeviceFee: measurementDeviceFeeItem,
      Total: (highEnergyItem?.Total ?? 0.0M)
        + (lowEnergyItem?.Total ?? 0.0M)
        + (energyItem?.Total ?? 0.0M)
        + (maxPowerItem?.Total ?? 0.0M)
        + (measurementDeviceFeeItem?.Total ?? 0.0M)
    );
  }

  private async Task<OzdsCalculationData> CreateCalculationDataAsync(
    DateTimeOffset from,
    DateTimeOffset to,
    OzdsBillingData billingData,
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

  private OzdsInvoiceData CreateInvoiceData(
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
    var renewableEnergyFeeAmount = (
      calculationData.SupplyExpenditure.EnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.LowEnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.HighEnergyItem?.Amount
      ?? 0.0M
    );
    var renewableEnergyFeeItem =
      renewableEnergyFeePrice == 0.0M
        ? null
        : new OzdsInvoiceFeeData(
          Amount: renewableEnergyFeeAmount,
          UnitPrice: renewableEnergyFeePrice,
          Total: renewableEnergyFeeAmount * renewableEnergyFeePrice
        );

    var businessUsageFeePrice =
      regulatoryAgencyCatalogueItem
        .RegulatoryAgencyCataloguePart
        .Value
        .BusinessUsageFee
        .Value ?? 0.0M;
    var businessUsageFeeAmount = (
      calculationData.SupplyExpenditure.EnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.LowEnergyItem?.Amount
      ?? 0.0M + calculationData.SupplyExpenditure.HighEnergyItem?.Amount
      ?? 0.0M
    );
    var businessUsageFeeItem =
      businessUsageFeePrice == 0.0M
        ? null
        : new OzdsInvoiceFeeData(
          Amount: businessUsageFeeAmount,
          UnitPrice: businessUsageFeePrice,
          Total: businessUsageFeeAmount * businessUsageFeePrice
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
      From: calculationData.From,
      To: calculationData.To,
      UsageFee: usageFee,
      SupplyFee: supplyFee,
      RenewableEnergyFee: renewableEnergyFeeItem,
      BusinessUsageFee: businessUsageFeeItem,
      Total: total,
      TaxRate: taxRate,
      Tax: tax,
      TotalWithTax: totalWithTax
    );
  }

  private async Task<OzdsInvoiceData> CreateInvoiceDataAsync(
    OzdsCalculationData calculationData,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem
  )
  {
    return CreateInvoiceData(calculationData, regulatoryAgencyCatalogueItem);
  }

  private OzdsReceiptData CreateReceiptData(OzdsInvoiceData invoiceData)
  {
    var renewableEnergyFee =
      invoiceData.RenewableEnergyFee == null
        ? null
        : new OzdsReceiptFeeData(
          Amount: invoiceData.RenewableEnergyFee.Amount,
          UnitPrice: invoiceData.RenewableEnergyFee.UnitPrice,
          Total: invoiceData.RenewableEnergyFee.Total
        );

    var businessUsageFee =
      invoiceData.BusinessUsageFee == null
        ? null
        : new OzdsReceiptFeeData(
          Amount: invoiceData.BusinessUsageFee.Amount,
          UnitPrice: invoiceData.BusinessUsageFee.UnitPrice,
          Total: invoiceData.BusinessUsageFee.Total
        );

    return new OzdsReceiptData(
      From: invoiceData.From,
      To: invoiceData.To,
      UsageFee: invoiceData.UsageFee,
      SupplyFee: invoiceData.SupplyFee,
      RenewableEnergyFee: renewableEnergyFee,
      BusinessUsageFee: businessUsageFee,
      Total: invoiceData.Total,
      TaxRate: invoiceData.TaxRate,
      Tax: invoiceData.Tax,
      TotalWithTax: invoiceData.TotalWithTax
    );
  }

  private async Task<OzdsReceiptData> CreateReceiptDataAsync(
    OzdsInvoiceData invoiceData
  )
  {
    return CreateReceiptData(invoiceData);
  }

  protected OzdsBillingFactory(IContentManager contentManager, ISession session)
  {
    _contnetManager = contentManager;
    _session = session;
  }

  private readonly IContentManager _contnetManager;

  private readonly ISession _session;
}
