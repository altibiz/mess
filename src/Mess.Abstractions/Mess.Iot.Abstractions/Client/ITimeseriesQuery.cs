namespace Mess.Iot.Abstractions.Client;

public interface ITimeseriesQuery
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
