using Mess.Billing.Abstractions;
using Mess.Iot.Abstractions.Models;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Billing;

public abstract class OzdsBillingFactory : IBillingFactory
{
  public abstract string ContentType { get; }

  protected abstract OzdsCalculationData CreateCalculationData(
    ContentItem measurementDeviceItem,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    OperatorCatalogueItem usageCatalogueItem,
    OperatorCatalogueItem supplyCatalogueItem
  );

  protected abstract Task<OzdsCalculationData> CreateCalculationDataAsync(
    ContentItem measurementDeviceItem,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    OperatorCatalogueItem usageCatalogueItem,
    OperatorCatalogueItem supplyCatalogueItem
  );

  protected abstract OzdsInvoiceData CreateInvoiceData(
    OzdsCalculationData calculationData
  );

  protected abstract Task<OzdsInvoiceData> CreateInvoiceDataAsync(
    OzdsCalculationData calculationData
  );

  protected abstract OzdsReceiptData CreateReceiptData(
    OzdsCalculationData calculationData
  );

  protected abstract Task<OzdsReceiptData> CreateReceiptDataAsync(
    OzdsCalculationData calculationData
  );

  public ContentItem CreateInvoice(ContentItem contentItem)
  {
    var measurementDevicePart =
      contentItem.As<MeasurementDevicePart>()
      ?? throw new NullReferenceException("MeasurementDevicePart is null");
    var ozdsMeasurementDevicePart =
      contentItem.As<OzdsMeasurementDevicePart>()
      ?? throw new NullReferenceException("OzdsMeasurementDevicePart is null");

    var unitItem =
      _contnetManager
        .GetContentAsync<DistributionSystemUnitItem>(
          ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId
        )
        .Result
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      _contnetManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId
        )
        .Result
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      _contnetManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId
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
          systemItem.ClosedDistributionSystemPart.Value.UsageCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsMeasurementDevicePart.SupplyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Supply catalogue not found");

    var calculationData = CreateCalculationData(
      contentItem,
      regulatoryAgencyCatalogueItem,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    var invoiceData = CreateInvoiceDataAsync(calculationData).Result;

    var invoiceItem = _contnetManager.NewContentAsync<OzdsInvoiceItem>().Result;
    invoiceItem.Alter(
      invoiceContentItem => invoiceContentItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.MeasurementDevice = contentItem;
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

  public async Task<ContentItem> CreateInvoiceAsync(ContentItem contentItem)
  {
    var measurementDevicePart =
      contentItem.As<MeasurementDevicePart>()
      ?? throw new NullReferenceException("MeasurementDevicePart is null");
    var ozdsMeasurementDevicePart =
      contentItem.As<OzdsMeasurementDevicePart>()
      ?? throw new NullReferenceException("OzdsMeasurementDevicePart is null");

    var unitItem =
      await _contnetManager.GetContentAsync<DistributionSystemUnitItem>(
        ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId
      )
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      await _contnetManager.GetContentAsync<ClosedDistributionSystemItem>(
        ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId
      )
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      await _contnetManager.GetContentAsync<DistributionSystemOperatorItem>(
        ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId
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
        systemItem.ClosedDistributionSystemPart.Value.UsageCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsMeasurementDevicePart.SupplyCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Supply catalogue not found");

    var calculationData = await CreateCalculationDataAsync(
      contentItem,
      regulatoryAgencyCatalogueItem,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    var invoiceData = await CreateInvoiceDataAsync(calculationData);

    var invoiceItem = await _contnetManager.NewContentAsync<OzdsInvoiceItem>();
    invoiceItem.Alter(
      invoiceContentItem => invoiceContentItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.MeasurementDevice = contentItem;
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
      contentItem.As<MeasurementDevicePart>()
      ?? throw new NullReferenceException("MeasurementDevicePart is null");
    var ozdsMeasurementDevicePart =
      contentItem.As<OzdsMeasurementDevicePart>()
      ?? throw new NullReferenceException("OzdsMeasurementDevicePart is null");

    var unitItem =
      _contnetManager
        .GetContentAsync<DistributionSystemUnitItem>(
          ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId
        )
        .Result
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      _contnetManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId
        )
        .Result
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      _contnetManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId
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
          systemItem.ClosedDistributionSystemPart.Value.UsageCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      _contnetManager
        .GetContentAsync<OperatorCatalogueItem>(
          ozdsMeasurementDevicePart.SupplyCatalogue.ContentItemIds.First()
        )
        .Result
      ?? throw new NullReferenceException("Supply catalogue not found");

    var calculationData = CreateCalculationData(
      contentItem,
      regulatoryAgencyCatalogueItem,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    var receiptData = CreateReceiptData(calculationData);

    var receiptItem = _contnetManager.NewContentAsync<OzdsReceiptItem>().Result;
    receiptItem.Alter(
      receiptItem => receiptItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.MeasurementDevice = contentItem;
        ozdsCalculationPart.RegulatoryAgencyCatalogue =
          regulatoryAgencyCatalogueItem;
        ozdsCalculationPart.UsageCatalogue = usageCatalogueItem;
        ozdsCalculationPart.SupplyCatalogue = supplyCatalogueItem;
        ozdsCalculationPart.Data = calculationData;
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
      contentItem.As<MeasurementDevicePart>()
      ?? throw new NullReferenceException("MeasurementDevicePart is null");
    var ozdsMeasurementDevicePart =
      contentItem.As<OzdsMeasurementDevicePart>()
      ?? throw new NullReferenceException("OzdsMeasurementDevicePart is null");

    var unitItem =
      await _contnetManager.GetContentAsync<DistributionSystemUnitItem>(
        ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId
      )
      ?? throw new NullReferenceException("Distribution system unit not found");
    var systemItem =
      await _contnetManager.GetContentAsync<ClosedDistributionSystemItem>(
        ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId
      )
      ?? throw new NullReferenceException(
        "Closed distribution system not found"
      );
    var operatorItem =
      await _contnetManager.GetContentAsync<DistributionSystemOperatorItem>(
        ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId
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
        systemItem.ClosedDistributionSystemPart.Value.UsageCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Usage catalogue not found");
    var supplyCatalogueItem =
      await _contnetManager.GetContentAsync<OperatorCatalogueItem>(
        ozdsMeasurementDevicePart.SupplyCatalogue.ContentItemIds.First()
      ) ?? throw new NullReferenceException("Supply catalogue not found");

    var calculationData = await CreateCalculationDataAsync(
      contentItem,
      regulatoryAgencyCatalogueItem,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    var receiptData = await CreateReceiptDataAsync(calculationData);

    var receiptItem = await _contnetManager.NewContentAsync<OzdsReceiptItem>();
    receiptItem.Alter(
      receiptItem => receiptItem.OzdsCalculationPart,
      ozdsCalculationPart =>
      {
        ozdsCalculationPart.MeasurementDevice = contentItem;
        ozdsCalculationPart.RegulatoryAgencyCatalogue =
          regulatoryAgencyCatalogueItem;
        ozdsCalculationPart.UsageCatalogue = usageCatalogueItem;
        ozdsCalculationPart.SupplyCatalogue = supplyCatalogueItem;
        ozdsCalculationPart.Data = calculationData;
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

  protected OzdsBillingFactory(IContentManager contentManager)
  {
    _contnetManager = contentManager;
  }

  private readonly IContentManager _contnetManager;
}
