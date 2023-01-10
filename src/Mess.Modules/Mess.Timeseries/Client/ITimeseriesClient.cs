using Mess.Timeseries.Entities;

namespace Mess.Timeseries.Client;

public interface ITimeseriesClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public Task AddMeasurementAsync(Measurement measurement);

  public void AddMeasurement(Measurement measurement);
}
