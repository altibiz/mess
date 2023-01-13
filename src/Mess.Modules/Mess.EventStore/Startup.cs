using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Util.Extensions.OrchardCore;
using Mess.Util.OrchardCore.Tenants;
using Mess.EventStore.Extensions.Microsoft;
using Mess.EventStore.Controllers;
using Mess.EventStore.Parsers.Egauge;
using Mess.EventStore.Client;
using OrchardCore.Admin;
using OrchardCore.Navigation;

namespace Mess.EventStore;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();
    services.AddScoped<INavigationProvider, AdminMenu>();

    services.AddMartenFromTenantGroups(
      Environment.IsDevelopment(),
      Configuration.GetOrchardAutoSetupTenantNamesGroupedByConnectionString()
    );

    services.AddScoped<EventStoreSession>();
    services.AddScoped<EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();

    services.AddSingleton<ITenantProvider, ShellTenantProvider>();

    services.AddSingleton<IEgaugeParser, EgaugeParser>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    services.UseEventStore();

    routes.MapAreaControllerRoute(
      name: "Mess.EventStore.Push.Egauge",
      areaName: "Mess.EventStore",
      pattern: "push/egauge",
      defaults: new
      {
        controller = typeof(PushController).ControllerName(),
        action = nameof(PushController.Egauge)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.EventStore.Admin",
      areaName: "Mess.EventStore",
      pattern: $"{Admin.AdminUrlPrefix}/EventStore",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.Index)
      }
    );
  }

  public Startup(
    IHostEnvironment environment,
    ILogger<Startup> logger,
    IConfiguration configuration,
    IOptions<AdminOptions> adminOptions
  )
  {
    Environment = environment;
    Logger = logger;
    Configuration = configuration;
    Admin = adminOptions.Value;
  }

  private IHostEnvironment Environment { get; }
  private ILogger Logger { get; }
  private IConfiguration Configuration { get; }
  private AdminOptions Admin { get; }

  [RequireFeatures("Mess.Timeseries")]
  public class TimeseriesStartup : StartupBase
  {
    public override void Configure(
      IApplicationBuilder app,
      IEndpointRouteBuilder routes,
      IServiceProvider services
    )
    {
      services.UseTimeseriesProjection();
    }
  }
}
