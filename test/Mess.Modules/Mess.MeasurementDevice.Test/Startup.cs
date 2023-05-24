using Mess.EventStore.Test.Abstractions;
using Mess.Timeseries.Test.Abstractions;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.MeasurementDevice.Dispatchers;
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

    services.AddScoped<EgaugeMeasurementDispatcher>();
    services.AddScoped<IMeasurementDispatcher, EgaugeMeasurementDispatcher>();

    var measurementClient = new Mock<IMeasurementClient>();
    services.AddSingleton(measurementClient);
    services.AddSingleton(measurementClient.Object);
  }
}
