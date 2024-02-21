namespace Mess.Ozds.Abstractions.Timeseries;

public interface IOzdsTimeseriesClient : IOzdsTimeseriesQuery
{
  public void AddAbbMeasurement(AbbMeasurement measurement);

  public Task AddAbbMeasurementAsync(AbbMeasurement measurement);

  public void AddSchneiderMeasurement(SchneiderMeasurement measurement);

  public Task AddSchneiderMeasurementAsync(SchneiderMeasurement measurement);

  public void AddBulkAbbMeasurement(IEnumerable<AbbMeasurement> measurements);

  public Task AddBulkAbbMeasurementAsync(IEnumerable<AbbMeasurement> measurements);

  public void AddBulkSchneiderMeasurement(IEnumerable<SchneiderMeasurement> measurements);

  public Task AddBulkSchneiderMeasurementAsync(IEnumerable<SchneiderMeasurement> measurements);
}
