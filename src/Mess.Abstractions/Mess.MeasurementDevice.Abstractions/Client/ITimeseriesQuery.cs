namespace Mess.MeasurementDevice.Abstractions.Client;

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

  public Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  );

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  );
}
