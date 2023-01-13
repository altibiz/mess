using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Data.Migration;
using Mess.Timeseries.Client;
using Mess.Util.OrchardCore.Tenants;
using Mess.Timeseries.Controllers;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.Timeseries;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();
    services.AddScoped<
      IDataMigration,
      Mess.Timeseries.Migrations.OrchardCore.Migrations
    >();

    // NOTE: use in scoped services
    services.AddDbContext<TimeseriesContext>();
    services.AddScoped<ITimeseriesMigrator, TimeseriesMigrator>();

    services.AddScoped<ITimeseriesConnection, TimeseriesConnection>();
    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();

    services.AddSingleton<ITenantProvider, ShellTenantProvider>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    using var scope = services.CreateScope();

    var client = scope.ServiceProvider.GetRequiredService<ITimeseriesClient>();
    var connected = client.CheckConnection();
    if (!connected)
    {
      throw new InvalidOperationException("Timeseries client not connected");
    }

    var migrator =
      scope.ServiceProvider.GetRequiredService<ITimeseriesMigrator>();
    migrator.Migrate();

    routes.MapAreaControllerRoute(
      name: "Timeseries",
      areaName: "Mess.Timeseries",
      pattern: "timeseries",
      defaults: new
      {
        controller = typeof(TimeseriesController).ControllerName(),
        action = nameof(TimeseriesController.Index)
      }
    );
  }
}
