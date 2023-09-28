namespace Mess.Iot.Abstractions.Client;

public interface ITimeseriesClient : ITimeseriesQuery
{
  public void AddEgaugeMeasurement(EgaugeMeasurement model);

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model);
}