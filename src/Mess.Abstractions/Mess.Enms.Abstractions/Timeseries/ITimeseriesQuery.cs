namespace Mess.Enms.Abstractions.Timeseries;

public interface IEnmsTimeseriesQuery
{
  public Task<IReadOnlyList<EgaugeMeasurement>> GetEgaugeMeasurementsAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public IReadOnlyList<EgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );
}
