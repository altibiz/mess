using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Services;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Billing;

public class AbbBillingFactory : OzdsIotDeviceBillingFactory<AbbIotDeviceItem>
{
  private readonly IOzdsTimeseriesQuery _query;

  public AbbBillingFactory(
    IContentManager contentManager,
    IOzdsTimeseriesQuery query
  )
    : base(contentManager)
  {
    _query = query;
  }

  protected override OzdsIotDeviceBillingData? FetchBillingData(
    AbbIotDeviceItem measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _query.GetAbbBillingData(
      measurementDeviceItem.IotDevicePart.Value.DeviceId.Text,
      fromDate,
      toDate
    );
  }

  protected override async Task<OzdsIotDeviceBillingData?> FetchBillingDataAsync(
    AbbIotDeviceItem measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _query.GetAbbBillingDataAsync(
      measurementDeviceItem.IotDevicePart.Value.DeviceId.Text,
      fromDate,
      toDate
    );
  }
}
