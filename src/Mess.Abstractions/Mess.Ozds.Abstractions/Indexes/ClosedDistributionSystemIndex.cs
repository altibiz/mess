using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class ClosedDistributionSystemIndex : MapIndex
{
  public string ClosedDistributionSystemContentItemId { get; set; } = default!;

  public string DistributionSystemOperatorContentItemId { get; set; } =
    default!;
}
