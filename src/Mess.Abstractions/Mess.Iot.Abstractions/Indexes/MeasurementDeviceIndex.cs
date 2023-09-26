using YesSql.Indexes;

namespace Mess.Iot.Abstractions.Indexes;

public class MeasurementDeviceIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;
  public string DeviceId { get; set; } = default!;
}
