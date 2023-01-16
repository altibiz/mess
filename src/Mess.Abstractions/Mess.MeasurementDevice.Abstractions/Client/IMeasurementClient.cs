using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Client;

public interface IMeasurementClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void AddEgaugeMeasurement(EgaugeMeasurementModel model);

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurementModel model);
}
