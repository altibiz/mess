using Mess.Event.Test.Abstractions.Extensions;
using Mess.Timeseries.Test.Abstractions.Extensions;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Iot;
using Moq;
using Mess.Iot.Abstractions.Timeseries;

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
    services.AddScoped<IIotPushHandler, EgaugePushHandler>();

    var measurementClient = new Mock<IIotTimeseriesClient>();
    services.AddSingleton(measurementClient);
    services.AddSingleton(measurementClient.Object);
  }
}
