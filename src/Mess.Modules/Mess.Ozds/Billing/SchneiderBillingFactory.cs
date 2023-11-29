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
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _query.GetSchneiderBillingData(
      measurementDeviceItem.IotDevicePart.Value.DeviceId.Text,
      fromDate,
      toDate
    );
  }

  protected override async Task<OzdsBillingData?> FetchBillingDataAsync(
    SchneiderIotDeviceItem measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _query.GetSchneiderBillingDataAsync(
      measurementDeviceItem.IotDevicePart.Value.DeviceId.Text,
      fromDate,
      toDate
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
