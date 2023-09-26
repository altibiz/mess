using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OzdsMeasurementDeviceIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string DeviceId { get; set; } = default!;

  public string ClosedDistributionSystemContentItemId { get; set; } = default!;

  public string[] ClosedDistributionSystemRepresentativeUserIds { get; set; } =
    default!;

  public string DistributionSystemOperatorContentItemId { get; set; } =
    default!;

  public string[] DistributionSystemOperatorRepresentativeUserIds { get; set; } =
    default!;

  public string DistributionSystemUnitContentItemId { get; set; } = default!;

  public string[] DistributionSystemUnitRepresentativeUserIds { get; set; } =
    default!;
}
