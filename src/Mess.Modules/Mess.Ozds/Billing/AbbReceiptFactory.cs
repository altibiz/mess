using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Receipts;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class AbbReceiptFactory : IReceiptFactory
{
  public const string ReceiptFactoryContentType = "AbbMeasurementDevice";

  public string ContentType => ReceiptFactoryContentType;

  public Receipt Create(ContentItem contentItem, ContentItem invoiceContentItem)
  {
    var abbMeasurementDevice =
      contentItem.AsContent<AbbMeasurementDeviceItem>();
    var invoice = invoiceContentItem.AsContent<InvoiceItem>();

    return new Receipt(
      BillableContnetItemId: abbMeasurementDevice.ContentItemId,
      IssuerContentItemId: abbMeasurementDevice
        .OzdsMeasurementDevicePart
        .Value
        .DistributionSystemOperatorContentItemId,
      RecipientContentItemId: abbMeasurementDevice
        .OzdsMeasurementDevicePart
        .Value
        .DistributionSystemUnitContentItemId,
      PartyContentItemIds: new[]
      {
        abbMeasurementDevice
          .OzdsMeasurementDevicePart
          .Value
          .DistributionSystemOperatorContentItemId,
        abbMeasurementDevice
          .OzdsMeasurementDevicePart
          .Value
          .DistributionSystemUnitContentItemId,
        abbMeasurementDevice
          .OzdsMeasurementDevicePart
          .Value
          .ClosedDistributionSystemContentItemId,
      },
      InvoiceContentItemId: invoice.ContentItemId,
      Id: Guid.NewGuid(),
      IssuedTimestamp: DateTime.UtcNow,
      Sections: new ReceiptSection[] { },
      Total: default
    );
  }

  public async Task<Receipt> CreateAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
    var abbMeasurementDevice =
      contentItem.AsContent<AbbMeasurementDeviceItem>();
    var invoice = invoiceContentItem.AsContent<InvoiceItem>();

    return new Receipt(
      BillableContnetItemId: abbMeasurementDevice.ContentItemId,
      IssuerContentItemId: abbMeasurementDevice
        .OzdsMeasurementDevicePart
        .Value
        .DistributionSystemOperatorContentItemId,
      RecipientContentItemId: abbMeasurementDevice
        .OzdsMeasurementDevicePart
        .Value
        .DistributionSystemUnitContentItemId,
      PartyContentItemIds: new[]
      {
        abbMeasurementDevice
          .OzdsMeasurementDevicePart
          .Value
          .DistributionSystemOperatorContentItemId,
        abbMeasurementDevice
          .OzdsMeasurementDevicePart
          .Value
          .DistributionSystemUnitContentItemId,
        abbMeasurementDevice
          .OzdsMeasurementDevicePart
          .Value
          .ClosedDistributionSystemContentItemId,
      },
      InvoiceContentItemId: invoice.ContentItemId,
      Id: Guid.NewGuid(),
      IssuedTimestamp: DateTime.UtcNow,
      Sections: new ReceiptSection[] { },
      Total: default
    );
  }
}
