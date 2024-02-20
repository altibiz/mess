using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
  public Task<ClosedDistributionSystemDiagnostics> GetClosedDistributionSystemDiagnostics(
    List<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
