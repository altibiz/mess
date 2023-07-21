using Mess.EventStore.Test.Abstractions;
using Mess.Timeseries.Test.Abstractions;
using Mess.MeasurementDevice.Abstractions.Pushing;
using Mess.MeasurementDevice.Pushing;
using Moq;
using Mess.MeasurementDevice.Abstractions.Client;

namespace Mess.MeasurementDevice.Test;

public class Startup : Mess.OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.AddTestEventStore();
    services.AddTestTimeseriesStore();

    services.AddScoped<EgaugePushHandler>();
    services.AddScoped<IMeasurementDevicePushHandler, EgaugePushHandler>();

    var measurementClient = new Mock<ITimeseriesClient>();
    services.AddSingleton(measurementClient);
    services.AddSingleton(measurementClient.Object);
  }
}
