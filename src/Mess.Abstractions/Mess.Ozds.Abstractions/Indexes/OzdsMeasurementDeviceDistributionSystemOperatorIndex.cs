using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OzdsMeasurementDeviceDistributionSystemOperatorIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; } = default!;

  public string DistributionSystemOperatorContentItemId { get; set; } =
    default!;

  public string DistributionSystemOperatorRepresentativeUserId { get; set; } =
    default!;
}
