using Mess.MeasurementDevice.Parsers.Egauge;
using Mess.Test.Extensions.Microsoft;
using Mess.EventStore.Test.Abstractions;
using Mess.Timeseries.Test.Abstractions;
using Mess.MeasurementDevice.Abstractions.Parsers;

namespace Mess.MeasurementDevice.Test;

public class Startup : Mess.OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.RegisterTenantFixture();
    services.RegisterTestEventStore();
    services.RegisterTestTimeseriesStore();

    services.AddScoped<EgaugeParser>();
    services.AddScoped<IMeasurementParser, EgaugeParser>();
  }
}
