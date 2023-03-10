using Mess.Test.Extensions.Microsoft;
using Mess.EventStore.Test.Abstractions;
using Mess.Timeseries.Test.Abstractions;

namespace Mess.Chart.Test;

public class Startup : Mess.Test.Startup
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
  }
}
