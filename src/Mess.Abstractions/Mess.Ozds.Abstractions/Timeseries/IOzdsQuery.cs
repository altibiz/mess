using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
  public Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<
    IReadOnlyList<SchneiderMeasurement>
  > GetSchneiderMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<SchneiderMeasurement> GetSchneiderMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
