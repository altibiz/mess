using Microsoft.Extensions.DependencyInjection;
using Mess.MeasurementDevice.Abstractions.Storage;

namespace Mess.MeasurementDevice.Storage;

public class MeasurementStorageStrategyLookup
  : IMeasurementStorageStrategyLookup
{
  public IMeasurementStorageStrategy? Get(string id) =>
    _services
      .GetServices<IMeasurementStorageStrategy>()
      .FirstOrDefault(storageStrategy => storageStrategy.Id == id);

  public MeasurementStorageStrategyLookup(IServiceProvider services)
  {
    _services = services;
  }

  private readonly IServiceProvider _services;
}
