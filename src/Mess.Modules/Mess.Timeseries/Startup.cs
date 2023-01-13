using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Admin;
using OrchardCore.Navigation;
using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Tenants;
using Mess.OrchardCore.Tenants;
using Mess.Timeseries.Controllers;
using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Client;

namespace Mess.Timeseries;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();
    services.AddScoped<INavigationProvider, AdminMenu>();

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
      name: "Mess.Timeseries.Admin",
      areaName: "Mess.Timeseries",
      pattern: $"{Admin.AdminUrlPrefix}/timeseries",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.Index)
      }
    );
  }

  public Startup(IOptions<AdminOptions> adminOptions)
  {
    Admin = adminOptions.Value;
  }

  private AdminOptions Admin { get; }
}
