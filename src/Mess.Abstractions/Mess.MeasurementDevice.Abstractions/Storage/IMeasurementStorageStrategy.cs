using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Storage;

public interface IMeasurementStorageStrategy
{
  public string Id { get; }

  public string? Store(ParsedMeasurementModel parsedMeasurement);

  public Task<string?> StoreAsync(ParsedMeasurementModel parsedMeasurement);
}
