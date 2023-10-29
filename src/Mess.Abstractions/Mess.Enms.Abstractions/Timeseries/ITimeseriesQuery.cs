namespace Mess.Enms.Abstractions.Timeseries;

public interface IEnmsTimeseriesQuery
{
  public Task<IReadOnlyList<EgaugeMeasurement>> GetEgaugeMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<EgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
