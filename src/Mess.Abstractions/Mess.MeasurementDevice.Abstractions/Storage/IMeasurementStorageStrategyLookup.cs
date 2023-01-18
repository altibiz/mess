namespace Mess.MeasurementDevice.Abstractions.Storage;

public interface IMeasurementStorageStrategyLookup
{
  public IMeasurementStorageStrategy? Get(string id);
}
