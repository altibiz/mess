using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Util.Extensions.OrchardCore;
using Mess.Util.OrchardCore.Tenants;
using Mess.EventStore.Extensions.Microsoft;
using Mess.EventStore.Controllers;

using Mess.EventStore.Client;

namespace Mess.EventStore;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();
    services.AddScoped<IDataMigration, Migrations>();

    services.AddMartenFromTenantGroups(
      Environment.IsDevelopment(),
      Configuration.GetOrchardAutoSetupTenantNamesGroupedByConnectionString()
    );

    services.AddScoped<EventStoreSession>();
    services.AddScoped<EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();

    services.AddSingleton<ITenantProvider, ShellTenantProvider>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    var client = services.GetRequiredService<IEventStoreClient>();
    var connected = client.CheckConnection();
    if (!connected)
    {
      throw new InvalidOperationException("EventStore client not connected");
    }

    routes.MapAreaControllerRoute(
      name: "Eguage",
      areaName: "Mess.EventStore",
      pattern: "push/egauge",
      defaults: new
      {
        controller = typeof(PushController).ControllerName(),
        action = nameof(PushController.Egauge)
      }
    );
  }

  public Startup(
    IHostEnvironment environment,
    ILogger<Startup> logger,
    IConfiguration configuration
  )
  {
    Environment = environment;
    Logger = logger;
    Configuration = configuration;
  }

  private IHostEnvironment Environment { get; }
  private ILogger Logger { get; }
  private IConfiguration Configuration { get; }
}
