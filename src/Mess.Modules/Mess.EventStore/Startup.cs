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
using Mess.OrchardCore.Tenants;
using Mess.OrchardCore.Extensions.Microsoft;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;
using Mess.EventStore.Controllers;
using Mess.EventStore.Services;
using Mess.Tenants;

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
      _environment.IsDevelopment(),
      // TODO: from shell settings somehow
      _configuration.GetOrchardCoreAutoSetupTenantNamesGroupedByConnectionString()
    );

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();

    services.AddSingleton<ITenants, ShellTenants>();
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

    var projection = services.GetRequiredService<Projection>();
    projection.Services = services;

    routes.MapAreaControllerRoute(
      name: "Mess.EventStore.AdminController.Index",
      areaName: "Mess.EventStore",
      pattern: $"{_adminOptions.AdminUrlPrefix}/EventStore",
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
    _environment = environment;
    _logger = logger;
    _configuration = configuration;
    _adminOptions = adminOptions.Value;
  }

  private readonly IHostEnvironment _environment;
  private readonly ILogger _logger;
  private readonly IConfiguration _configuration;
  private readonly AdminOptions _adminOptions;
}
