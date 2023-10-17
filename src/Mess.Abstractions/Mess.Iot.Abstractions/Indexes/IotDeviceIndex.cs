using YesSql.Indexes;

namespace Mess.Iot.Abstractions.Indexes;

public class IotDeviceIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; } = default!;
}
