using Mess.Timeseries.Test.Abstractions.Extensions;

namespace Mess.Timeseries.Test;

public class Startup : OrchardCore.Test.Startup
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
