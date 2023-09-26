using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsMeasurementDevicePart : ContentPart
{
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
