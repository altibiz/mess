using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Timeseries.Client;

public interface IMeasurementClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void AddEgaugeMeasurement(EgaugeMeasurementModel model);

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurementModel model);

  public Task<IReadOnlyList<EgaugeMeasurementModel>> GetEgaugeMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  );

  public IReadOnlyList<EgaugeMeasurementModel> GetEgaugeMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  );
}
