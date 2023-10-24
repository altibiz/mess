namespace Mess.Enms.Abstractions.Timeseries;

public interface IEnmsTimeseriesClient : IEnmsTimeseriesQuery
{
  public void AddEgaugeMeasurement(EgaugeMeasurement model);

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model);
}
