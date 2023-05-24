using Mess.MeasurementDevice.Abstractions.Dispatchers;

namespace Mess.MeasurementDevice.Abstractions.Client;

public interface IMeasurementQuery
{
  public Task<
    IReadOnlyList<DispatchedEgaugeMeasurement>
  > GetEgaugeMeasurementsAsync(string source, DateTime beginning, DateTime end);

  public IReadOnlyList<DispatchedEgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  );
}
