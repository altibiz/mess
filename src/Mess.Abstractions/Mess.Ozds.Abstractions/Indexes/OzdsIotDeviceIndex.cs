using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OzdsIotDeviceIndex : MapIndex
{
  public string OzdsIotDeviceContentItemId { get; set; } = default!;

  public string DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; } = default!;

  public string DistributionSystemUnitContentItemId { get; set; } = default!;

  public string ClosedDistributionSystemContentItemId { get; set; } = default!;

  public string DistributionSystemOperatorContentItemId { get; set; } =
    default!;
}
