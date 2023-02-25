using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Client;
using Mess.Xunit.Extensions.Microsoft;
using Mess.Timeseries.Test.Abstractions;

namespace Mess.Timeseries.Test;

public class Startup : Mess.Xunit.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.RegisterTestTenants();
    services.RegisterTestTimeseriesStore();

    services.AddScoped<ITimeseriesConnection, TimeseriesConnection>();
    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();
  }
}
