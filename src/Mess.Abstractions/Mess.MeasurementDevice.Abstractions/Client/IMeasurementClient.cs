using Mess.MeasurementDevice.Abstractions.Dispatchers;

namespace Mess.MeasurementDevice.Abstractions.Client;

public interface IMeasurementClient : IMeasurementQuery
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void AddEgaugeMeasurement(DispatchedEgaugeMeasurement model);

  public Task AddEgaugeMeasurementAsync(DispatchedEgaugeMeasurement model);
}
