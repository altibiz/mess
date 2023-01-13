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
using OrchardCore.Admin;
using OrchardCore.Navigation;
using Mess.EventStore.Extensions.Microsoft;
using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Tenants;
using Mess.OrchardCore.Tenants;
using Mess.EventStore.Abstractions.Parsers.Egauge;
using Mess.EventStore.Parsers.Egauge;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;
using Mess.EventStore.Controllers;

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

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
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
