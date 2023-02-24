using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.MeasurementDevice.Abstractions.Storage;
using Mess.MeasurementDevice.EventStore.Storage;
using Mess.EventStore.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.EventStore.Events;

namespace Mess.MeasurementDevice.EventStore;

[RequireFeatures("Mess.EventStore")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.RegisterProjectionDispatcher<MeasurementProjectionDispatcher>();

    services.AddSingleton<
      IMeasurementStorageStrategy,
      EgaugeEventStoreStorageStrategy
    >();
  }
}
