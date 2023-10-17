using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql;

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

  public AbbBillingFactory(
    IContentManager contentManager,
    ISession session,
    IOzdsTimeseriesQuery query
  )
    : base(contentManager, session)
  {
    _query = query;
  }

  private readonly IOzdsTimeseriesQuery _query;
}
