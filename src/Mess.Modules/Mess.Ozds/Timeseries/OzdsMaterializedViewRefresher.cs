using Mess.Timeseries.Abstractions.Services;

namespace Mess.Ozds.Timeseries;

public class OzdsMaterializedViewRefresher : MaterializedViewRefresher<OzdsTimeseriesDbContext>
{
  protected override IEnumerable<ViewDescriptor> Views => new[]
  {
    new ViewDescriptor("MonthlyBoundsEnergy", false),
    new ViewDescriptor("QuarterHourAveragePower", false),
  };
}
