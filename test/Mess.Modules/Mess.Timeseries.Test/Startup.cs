using Mess.Timeseries.Test.Abstractions;

namespace Mess.Timeseries.Test;

public class Startup : Mess.OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.AddTestTimeseriesStore();
  }
}
