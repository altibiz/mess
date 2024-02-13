using Mess.Billing.Abstractions.Services;
using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Services;
using OrchardCore.ContentManagement;
using YesSql;
using ISession = YesSql.ISession;

namespace Mess.Ozds.Abstractions.Billing;

// TODO: shorten the hell out of this thing

public class OzdsClosedDistributionUnitBillingFactory : IBillingFactory
{
  private readonly IContentManager _contentManager;

  private readonly ISession _session;

  private readonly List<IOzdsIotDeviceBillingFactory> _iotDeviceBillingFactories;

  public OzdsClosedDistributionUnitBillingFactory(
    IContentManager contentManager,
    ISession session,
    IEnumerable<IOzdsIotDeviceBillingFactory> iotDeviceBillingFactories
  )
  {
    _contentManager = contentManager;
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
    var system = await _contentManager
      .GetContentAsync<ClosedDistributionSystemItem>(unit.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Closed distribution system {unit.ContainedPart.Value.ListContentItemId} not found");
    var @operator = await _contentManager
      .GetContentAsync<DistributionSystemOperatorItem>(system.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Distribution system operator {system.ContainedPart.Value.ListContentItemId} not found");

    var iotDevices = await _session
      .Query<ContentItem, OzdsIotDeviceIndex>()
      .Where(index => index.DistributionSystemUnitContentItemId == contentItem.ContentItemId)
      .ListAsync();

    var calculations = new List<OzdsCalculationData>();
    foreach (var iotDevice in iotDevices)
    {
      if (iotDevice.As<IotDevicePart>()?.IsMessenger is true)
      {
        continue;
      }

      var iotDeviceBillingFactory = _iotDeviceBillingFactories
        .FirstOrDefault(iotDeviceBillingFactory => iotDeviceBillingFactory.IsApplicable(iotDevice))
          ?? throw new InvalidOperationException($"Billing factory not found for {iotDevice}");

      var calculation = await iotDeviceBillingFactory
        .CreateCalculationAsync(
          unit,
          system,
          @operator,
          iotDevice,
          fromDate,
          toDate
        );

      calculations.Add(calculation);
    }

    var invoiceData = await CreateInvoiceDataAsync(
      @operator,
      system,
      unit,
      calculations,
      fromDate,
      toDate
    );

    var invoice = await _contentManager
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
        ozdsInvoicePart.Data = invoiceData;
      }
    );

    unit.Alter(
      unit => unit.DistributionSystemUnitPart,
      part =>
      {
        part.Consumption += invoice.OzdsInvoicePart.Value.Data.TotalWithTax;
      }
    );
    await _contentManager.UpdateAsync(unit);

    system.Alter(
      system => system.ClosedDistributionSystemPart,
      part =>
      {
        part.Consumption += invoice.OzdsInvoicePart.Value.Data.TotalWithTax;
      }
    );
    await _contentManager.UpdateAsync(system);

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
    var system = await _contentManager
      .GetContentAsync<ClosedDistributionSystemItem>(unit.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Closed distribution system {unit.ContainedPart.Value.ListContentItemId} not found");
    var @operator = await _contentManager
      .GetContentAsync<DistributionSystemOperatorItem>(system.ContainedPart.Value.ListContentItemId)
        ?? throw new InvalidOperationException($"Distribution system operator {system.ContainedPart.Value.ListContentItemId} not found");

    var receiptData = await CreateReceiptDataAsync(
      invoice.OzdsInvoicePart.Value.Data
    );

    var receipt = await _contentManager
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
        ozdsReceiptPart.Data = receiptData;
      }
    );

    return receipt;
  }

  private static OzdsInvoiceData CreateInvoiceData(
    ContentItem distributionSystemOperator,
    ContentItem closedDistributionSystem,
    ContentItem distributionSystemUnit,
    List<OzdsCalculationData> calculations,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var usageExpenditure = calculations
      .Aggregate(
        new OzdsInvoiceUsageExpenditureData(
          0,
          0,
          0,
          0,
          0,
          0,
          0
        ),
        (expenditure, calculation) =>
          new OzdsInvoiceUsageExpenditureData(
            expenditure.HighEnergyFee + (calculation.UsageExpenditure.HighEnergyItem?.Total ?? 0.0M),
            expenditure.LowEnergyFee + (calculation.UsageExpenditure.LowEnergyItem?.Total ?? 0.0M),
            expenditure.EnergyFee + (calculation.UsageExpenditure.EnergyItem?.Total ?? 0.0M),
            expenditure.ReactiveEnergyFee + (calculation.UsageExpenditure.ReactiveEnergyItem?.Total ?? 0.0M),
            expenditure.MaxPowerFee + (calculation.UsageExpenditure.MaxPowerItem?.Total ?? 0.0M),
            expenditure.IotDeviceFee + (calculation.UsageExpenditure.IotDeviceFee?.Total ?? 0.0M),
            expenditure.Total + calculation.UsageExpenditure.Total
          )
    );
    var supplyExpenditure = calculations
      .Aggregate(
        new OzdsInvoiceSupplyExpenditureData(
          0,
          0,
          0,
          0,
          0
        ),
        (expenditure, calculation) =>
          new OzdsInvoiceSupplyExpenditureData(
            expenditure.HighEnergyFee + (calculation.SupplyExpenditure.HighEnergyItem?.Total ?? 0.0M),
            expenditure.LowEnergyFee + (calculation.SupplyExpenditure.LowEnergyItem?.Total ?? 0.0M),
            expenditure.RenewableEnergyFee + (calculation.SupplyExpenditure.RenewableEnergyFee?.Total ?? 0.0M),
            expenditure.BusinessUsageFee + (calculation.SupplyExpenditure.BusinessUsageFee?.Total ?? 0.0M),
            expenditure.Total + calculation.SupplyExpenditure.Total
          )
    );


    var total = usageExpenditure.Total + supplyExpenditure.Total;
    // TODO: from catalogue
    var taxRate = 0.13M;
    var tax = Math.Round(total * taxRate, 2);
    var totalWithTax = total + tax;

    return new OzdsInvoiceData(
      distributionSystemOperator,
      closedDistributionSystem,
      distributionSystemUnit,
      fromDate,
      toDate,
      usageExpenditure,
      supplyExpenditure,
      total,
      taxRate,
      tax,
      totalWithTax
    );
  }

  private static async Task<OzdsInvoiceData> CreateInvoiceDataAsync(
    ContentItem distributionSystemOperator,
    ContentItem closedDistributionSystem,
    ContentItem distributionSystemUnit,
    List<OzdsCalculationData> calculations,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return CreateInvoiceData(
      distributionSystemOperator,
      closedDistributionSystem,
      distributionSystemUnit,
      calculations,
      fromDate,
      toDate
    );
  }

  private static OzdsReceiptData CreateReceiptData(
    OzdsInvoiceData invoiceData
  )
  {
    return new OzdsReceiptData(
      invoiceData.DistributionSystemOperator,
      invoiceData.ClosedDistributionSystem,
      invoiceData.DistributionSystemUnit,
      invoiceData.From,
      invoiceData.To,
      new OzdsReceiptUsageExpenditureData(
        invoiceData.UsageExpenditure.HighEnergyFee,
        invoiceData.UsageExpenditure.LowEnergyFee,
        invoiceData.UsageExpenditure.EnergyFee,
        invoiceData.UsageExpenditure.ReactiveEnergyFee,
        invoiceData.UsageExpenditure.MaxPowerFee,
        invoiceData.UsageExpenditure.IotDeviceFee,
        invoiceData.UsageExpenditure.Total
      ),
      new OzdsReceiptSupplyExpenditureData(
        invoiceData.SupplyExpenditure.HighEnergyFee,
        invoiceData.SupplyExpenditure.LowEnergyFee,
        invoiceData.SupplyExpenditure.RenewableEnergyFee,
        invoiceData.SupplyExpenditure.BusinessUsageFee,
        invoiceData.SupplyExpenditure.Total
      ),
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
