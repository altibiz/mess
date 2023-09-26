namespace Mess.Iot.Abstractions.Client;

public interface ITimeseriesQuery
{
  public Task<IReadOnlyList<EgaugeMeasurement>> GetEgaugeMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  );

  public IReadOnlyList<EgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  );
}
