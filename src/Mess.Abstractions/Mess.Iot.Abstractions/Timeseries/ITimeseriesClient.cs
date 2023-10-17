namespace Mess.Iot.Abstractions.Timeseries;

public interface IIotTimeseriesClient : IIotTimeseriesQuery
{
  public void AddEgaugeMeasurement(EgaugeMeasurement model);

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model);
}
