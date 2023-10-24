namespace Mess.Ozds.Abstractions.Timeseries;

public interface IOzdsTimeseriesClient : IOzdsTimeseriesQuery
{
  public void AddAbbMeasurement(AbbMeasurement model);

  public Task AddAbbMeasurementAsync(AbbMeasurement model);
}
