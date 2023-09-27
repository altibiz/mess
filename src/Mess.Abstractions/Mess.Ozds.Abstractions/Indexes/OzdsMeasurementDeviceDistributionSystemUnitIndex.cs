using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OzdsMeasurementDeviceDistributionSystemUnitIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; } = default!;

  public string DistributionSystemUnitContentItemId { get; set; } = default!;

  public string DistributionSystemUnitRepresentativeUserId { get; set; } =
    default!;
}
