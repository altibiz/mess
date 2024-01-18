using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Billing;

public class SchneiderBillingFactory
  : OzdsClosedDistributionUnitBillingFactory<SchneiderIotDeviceItem>
{
  private readonly IOzdsTimeseriesQuery _query;

  public SchneiderBillingFactory(
    IContentManager contentManager,
    ISession session,
    IOzdsTimeseriesQuery query
  )
    : base(contentManager, session)
  {
    _query = query;
  }

  protected override OzdsIotDeviceBillingData? FetchBillingData(
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

  protected override async Task<OzdsIotDeviceBillingData?> FetchBillingDataAsync(
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
}
