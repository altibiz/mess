using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OzdsMeasurementDeviceClosedDistributionSystemIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; } = default!;

  public string ClosedDistributionSystemContentItemId { get; set; } = default!;

  public string ClosedDistributionSystemRepresentativeUserId { get; set; } =
    default!;
}
