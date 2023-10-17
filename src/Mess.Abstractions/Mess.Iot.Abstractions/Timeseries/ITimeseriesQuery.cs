namespace Mess.Iot.Abstractions.Timeseries;

public interface IIotTimeseriesQuery
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
