using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Client;
using Mess.Test.Extensions.Microsoft;
using Mess.Timeseries.Test.Abstractions;

namespace Mess.Timeseries.Test;

public class Startup : Mess.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.RegisterTenantFixture();
    services.RegisterTestTimeseriesStore();

    services.AddScoped<ITimeseriesConnection, TimeseriesConnection>();
    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();
  }
}
