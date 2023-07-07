using YesSql.Indexes;

namespace Mess.Eor.Abstractions.Indexes;

public class EorMeasurementDeviceIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;
  public string DeviceId { get; set; } = default!;
  public string OwnerId { get; set; } = default!;
}
