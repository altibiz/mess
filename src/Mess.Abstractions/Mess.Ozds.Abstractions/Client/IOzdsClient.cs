namespace Mess.Ozds.Abstractions.Client;

public interface IOzdsClient : IOzdsQuery
{
  public void AddAbbMeasurement(AbbMeasurement model);

  public Task AddAbbMeasurementAsync(AbbMeasurement model);
}
