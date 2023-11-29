using Mess.Event.Test.Abstractions.Extensions;
using Mess.Timeseries.Test.Abstractions.Extensions;

namespace Mess.Chart.Test;

public class Startup : OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.AddTestEventStore();
    services.AddTestTimeseriesStore();
  }
}
