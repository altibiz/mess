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

public abstract class OzdsClosedDistributionUnitBillingFactory : IBillingFactory
{
  private readonly IContentManager _contnetManager;

  private readonly ISession _session;

  private readonly List<IOzdsIotDeviceBillingFactory> _iotDeviceBillingFactories;

  protected OzdsClosedDistributionUnitBillingFactory(
    IContentManager contentManager,
    ISession session,
    IEnumerable<IOzdsIotDeviceBillingFactory> iotDeviceBillingFactories
  )
  {
    _contnetManager = contentManager;
    _session = session;
    _iotDeviceBillingFactories = iotDeviceBillingFactories.ToList();
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == "DistributionSystemUnit";
  }

  public ContentItem CreateInvoice(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return CreateInvoiceAsync(
      contentItem,
      fromDate,
      toDate).Result;
  }

  public async Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var unit = contentItem
      .AsContent<DistributionSystemUnitItem>();
    var system = await _contnetManager
      .GetContentAsync<ClosedDistributionSystemItem>(unit.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Closed distribution system {unit.ContainedPart.Value.ListContentItemId} not found");
    var @operator = await _contnetManager
      .GetContentAsync<DistributionSystemOperatorItem>(system.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Distribution system operator {system.ContainedPart.Value.ListContentItemId} not found");

    var regulatoryAgencyCatalogueItem =
      _contnetManager
        .GetContentAsync<RegulatoryAgencyCatalogueItem>(
          @operator.DistributionSystemOperatorPart.Value
            .RegulatoryAgencyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException(
        "Regulatory agency catalogue not found"
      );

    var iotDevices = await _session
      .Query<ContentItem, OzdsIotDeviceIndex>()
      .Where(index => index.DistributionSystemUnitContentItemId == contentItem.ContentItemId)
      .ListAsync();

    var calculations = new List<OzdsCalculationData>();
    foreach (var iotDevice in iotDevices)
    {
      var iotDeviceBillingFactory = _iotDeviceBillingFactories
        .FirstOrDefault(iotDeviceBillingFactory => iotDeviceBillingFactory.IsApplicable(iotDevice))
          ?? throw new InvalidOperationException($"Billing factory not found for {iotDevice}");

      var calculation = await iotDeviceBillingFactory
        .CreateCalculationAsync(
          unit,
          system,
          @operator,
          regulatoryAgencyCatalogueItem,
          iotDevice,
          fromDate,
          toDate
        );

      calculations.Add(calculation);
    }

    var invoiceData = await CreateInvoiceDataAsync(
      calculations,
      regulatoryAgencyCatalogueItem,
      fromDate,
      toDate
    );

    var invoice = await _contnetManager
      .NewContentAsync<OzdsInvoiceItem>();
    invoice.Alter(
      invoiceContentItem => invoiceContentItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.Calculations = calculations;
      }
    );
    invoice.Alter(
      invoiceContentItem => invoiceContentItem.OzdsInvoicePart,
      ozdsInvoicePart =>
      {
        ozdsInvoicePart.DistributionSystemOperator = @operator;
        ozdsInvoicePart.ClosedDistributionSystem = system;
        ozdsInvoicePart.DistributionSystemUnit = unit;
        ozdsInvoicePart.Data = invoiceData;
      }
    );

    return invoice;
  }

  public ContentItem CreateReceipt(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
    return CreateReceiptAsync(
      contentItem,
      invoiceContentItem
    ).Result;
  }

  public async Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
    var invoice = invoiceContentItem.AsContent<OzdsInvoiceItem>();

    var unit = contentItem
      .AsContent<DistributionSystemUnitItem>();
    var system = await _contnetManager
      .GetContentAsync<ClosedDistributionSystemItem>(unit.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Closed distribution system {unit.ContainedPart.Value.ListContentItemId} not found");
    var @operator = await _contnetManager
      .GetContentAsync<DistributionSystemOperatorItem>(system.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Distribution system operator {system.ContainedPart.Value.ListContentItemId} not found");

    var regulatoryAgencyCatalogueItem =
      _contnetManager
        .GetContentAsync<RegulatoryAgencyCatalogueItem>(
          @operator.DistributionSystemOperatorPart.Value
            .RegulatoryAgencyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new InvalidOperationException(
        "Regulatory agency catalogue not found"
      );

    var receiptData = await CreateReceiptDataAsync(
      invoice.OzdsInvoicePart.Value.Data
    );

    var receipt = await _contnetManager
      .NewContentAsync<OzdsReceiptItem>();
    receipt.Alter(
      invoiceContentItem => invoiceContentItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.Calculations = invoice.OzdsCalculationPart.Value.Calculations;
      }
    );
    receipt.Alter(
      invoiceContentItem => invoiceContentItem.OzdsReceiptPart,
      ozdsReceiptPart =>
      {
        ozdsReceiptPart.DistributionSystemOperator = @operator;
        ozdsReceiptPart.ClosedDistributionSystem = system;
        ozdsReceiptPart.DistributionSystemUnit = unit;
        ozdsReceiptPart.Data = receiptData;
      }
    );

    return receipt;
  }

  private static OzdsInvoiceData CreateInvoiceData(
    List<OzdsCalculationData> calculations,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var usageFee = calculations.Sum(calculation => calculation.UsageExpenditure.Total);

    var supplyFee = calculations.Sum(calculation => calculation.SupplyExpenditure.Total);

    var renewableEnergyFeePrice =
      regulatoryAgencyCatalogueItem
        .RegulatoryAgencyCataloguePart
        .Value
        .RenewableEnergyFee
        .Value ?? 0.0M;
    var renewableEnergyFeeAmount = calculations.Sum(calculation =>
      calculation.SupplyExpenditure.EnergyItem?.Amount
      ?? 0.0M + calculation.SupplyExpenditure.LowEnergyItem?.Amount
      ?? 0.0M + calculation.SupplyExpenditure.HighEnergyItem?.Amount
      ?? 0.0M
    );
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
    var businessUsageFeeAmount = calculations.Sum(calculation =>
      calculation.SupplyExpenditure.EnergyItem?.Amount
      ?? 0.0M + calculation.SupplyExpenditure.LowEnergyItem?.Amount
      ?? 0.0M + calculation.SupplyExpenditure.HighEnergyItem?.Amount
      ?? 0.0M
    );
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
      fromDate,
      toDate,
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
    List<OzdsCalculationData> calculations,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return CreateInvoiceData(
      calculations,
      regulatoryAgencyCatalogueItem,
      fromDate,
      toDate
    );
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
