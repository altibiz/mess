using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Admin;
using OrchardCore.Navigation;
using Mess.Tenants;
using Mess.OrchardCore.Tenants;
using Mess.Timeseries.Controllers;
using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Client;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;

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

    services.RegisterTimeseriesDbContext<TimeseriesContext>();
    services.AddScoped<ITimeseriesMigrator, TimeseriesMigrator>();

    services.AddScoped<ITimeseriesConnection, TimeseriesConnection>();
    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();

    services.AddSingleton<ITenants, ShellTenants>();
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
      pattern: $"{_adminOptions.AdminUrlPrefix}/timeseries",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.Index)
      }
    );
  }

  public Startup(
    IOptions<AdminOptions> adminOptions,
    IConfiguration configuration
  )
  {
    _adminOptions = adminOptions.Value;
    _configuration = configuration;
  }

  private readonly AdminOptions _adminOptions;
  private readonly IConfiguration _configuration;
}
