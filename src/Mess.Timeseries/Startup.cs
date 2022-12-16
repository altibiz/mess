using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Data.Migration;
using Mess.Timeseries.Client;
using Mess.Util.OrchardCore.Tenants;

namespace Mess.Timeseries;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();
    services.AddScoped<IDataMigration, Migrations>();

    // TODO: change this to dependency injection with Npgsql 7
    // FIX: it must be run before any Npgsql operations
    // NpgsqlLogManager.Provider = new NLogLoggingProvider();
    // NpgsqlLogManager.IsParameterLoggingEnabled = true;

    services.AddScoped<TimeseriesConnection>();
    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();

    services.AddSingleton<ITenantProvider, ShellTenantProvider>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    var client = services.GetRequiredService<ITimeseriesClient>();
    var connected = client.CheckConnection();
    if (!connected)
    {
      throw new InvalidOperationException("Timeseries client not connected");
    }
  }
}
