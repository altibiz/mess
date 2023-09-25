using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OzdsMeasurementDeviceIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;
  public string DeviceId { get; set; } = default!;
}
