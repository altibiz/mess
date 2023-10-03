using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class DistributionSystemUnitIndex : MapIndex
{
  public string DistributionSystemUnitContentItemId { get; set; } = default!;

  public string ClosedDistributionSystemContentItemId { get; set; } = default!;

  public string DistributionSystemOperatorContentItemId { get; set; } =
    default!;
}
