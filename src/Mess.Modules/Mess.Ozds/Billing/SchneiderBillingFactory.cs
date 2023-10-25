using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Billing;

public class SchneiderBillingFactory
  : OzdsBillingFactory<SchneiderIotDeviceItem>
{
  protected override OzdsBillingData? FetchBillingData(
    SchneiderIotDeviceItem measurementDeviceItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    return _query.GetSchneiderBillingData(
      measurementDeviceItem.IotDevicePart.Value.DeviceId.Text,
      from,
      to
    );
  }

  protected override async Task<OzdsBillingData?> FetchBillingDataAsync(
    SchneiderIotDeviceItem measurementDeviceItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    return await _query.GetSchneiderBillingDataAsync(
      measurementDeviceItem.IotDevicePart.Value.DeviceId.Text,
      from,
      to
    );
  }

  public SchneiderBillingFactory(
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
