using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class AbbBillingFactory : OzdsBillingFactory<AbbMeasurementDeviceItem>
{
  protected override OzdsBillingData? FetchBillingData(
    AbbMeasurementDeviceItem measurementDeviceItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    return _query.GetAbbBillingData(
      measurementDeviceItem.MeasurementDevicePart.Value.DeviceId.Text,
      from,
      to
    );
  }

  protected override async Task<OzdsBillingData?> FetchBillingDataAsync(
    AbbMeasurementDeviceItem measurementDeviceItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    return await _query.GetAbbBillingDataAsync(
      measurementDeviceItem.MeasurementDevicePart.Value.DeviceId.Text,
      from,
      to
    );
  }

  public AbbBillingFactory(IContentManager contentManager, IOzdsQuery query)
    : base(contentManager)
  {
    _query = query;
  }

  private readonly IOzdsQuery _query;
}
