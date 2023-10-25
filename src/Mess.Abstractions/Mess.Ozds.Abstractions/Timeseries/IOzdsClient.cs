namespace Mess.Ozds.Abstractions.Timeseries;

public interface IOzdsTimeseriesClient : IOzdsTimeseriesQuery
{
  public void AddAbbMeasurement(AbbMeasurement model);

  public Task AddAbbMeasurementAsync(AbbMeasurement model);

  public void AddSchneiderMeasurement(SchneiderMeasurement model);

  public Task AddSchneiderMeasurementAsync(SchneiderMeasurement model);
}
