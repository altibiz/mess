using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
  public Task<(decimal? First, decimal? Last, DateTimeOffset FirstDate)> GetAbbLastMonthMeasurementsAsync(
    string source,
    DateTimeOffset startDate,
    DateTimeOffset endDate
  );

  public Task<(decimal? First, decimal? Last, DateTimeOffset FirstDate)> GetSchneiderLastMonthMeasurementsAsync(
    string source,
    DateTimeOffset startDate,
    DateTimeOffset endDate
  );
}
