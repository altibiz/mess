using Mess.Event.Test.Abstractions.Extensions;
using Mess.Timeseries.Test.Abstractions.Extensions;
using Mess.Iot.Abstractions.Pushing;
using Mess.Iot.Pushing;
using Moq;
using Mess.Iot.Abstractions.Client;

namespace Mess.Iot.Test;

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
