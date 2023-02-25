using Mess.MeasurementDevice.Parsers.Egauge;
using Mess.Xunit.Extensions.Microsoft;
using Mess.EventStore.Test.Abstractions;
using Mess.Timeseries.Test.Abstractions;
using Mess.MeasurementDevice.Abstractions.Parsers;

namespace Mess.MeasurementDevice.Test;

public class Startup : Mess.Xunit.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.RegisterTestTenants();
    services.RegisterTestEventStore();
    services.RegisterTestTimeseriesStore();

    services.AddScoped<EgaugeParser>();
    services.AddScoped<IMeasurementParser, EgaugeParser>();
  }
}
