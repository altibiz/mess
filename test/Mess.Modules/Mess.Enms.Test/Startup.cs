using Mess.Enms.Abstractions.Timeseries;
using Mess.Enms.Iot;
using Mess.Event.Test.Abstractions.Extensions;
using Mess.Iot.Abstractions.Services;
using Mess.Timeseries.Test.Abstractions.Extensions;
using Moq;

namespace Mess.Enms.Test;

public class Startup : Cms.Test.Startup
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

    var measurementClient = new Mock<IEnmsTimeseriesClient>();
    services.AddSingleton(measurementClient);
    services.AddSingleton(measurementClient.Object);
  }
}
