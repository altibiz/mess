using Mess.Timeseries.Abstractions.Services;

namespace Mess.Ozds.Timeseries;

public class OzdsMaterializedViewRefresher : MaterializedViewRefresher<OzdsTimeseriesDbContext>
{
  protected override string[] Views => new[]
  {
    "MonthlyBoundsEnergy",
    "QuarterHourAveragePower",
  };
}
