using Mess.Billing.Abstractions.Invoices;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class AbbInvoiceFactory : IInvoiceFactory
{
  public const string InvoiceFactoryContentType = "AbbMeasurementDevice";

  public string ContentType => InvoiceFactoryContentType;

  public Invoice Create(ContentItem contentItem)
  {
    var abbMeasurementDevice =
      contentItem.AsContent<AbbMeasurementDeviceItem>();

    return new(
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
      ReceiptContentItemId: null,
      Id: Guid.NewGuid(),
      IssuedTimestamp: DateTime.UtcNow,
      Sections: new InvoiceSection[] { },
      Total: default
    );
  }

  public async Task<Invoice> CreateAsync(ContentItem contentItem)
  {
    var abbMeasurementDevice =
      contentItem.AsContent<AbbMeasurementDeviceItem>();

    return new(
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
      ReceiptContentItemId: null,
      Id: Guid.NewGuid(),
      IssuedTimestamp: DateTime.UtcNow,
      Sections: new InvoiceSection[] { },
      Total: default
    );
  }
}
