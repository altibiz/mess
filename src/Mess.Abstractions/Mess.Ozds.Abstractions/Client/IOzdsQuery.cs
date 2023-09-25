namespace Mess.Ozds.Abstractions.Client;

public interface IOzdsQuery
{
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
