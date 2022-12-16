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
using Marten;
using Weasel.Core;
using Mess.Util.Extensions.OrchardCore;

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

    services
      .AddMarten(
        (StoreOptions options) =>
        {
          options.MultiTenantedDatabases(databases =>
          {
            foreach (
              var (
                connectionString,
                tenants
              ) in Configuration.GetAutoSetupTenantNamesGroupedByConnectionString()
            )
            {
              var tenantsArray = tenants.ToArray();
              if (tenantsArray.Length == 1)
              {
                databases.AddSingleTenantDatabase(
                  connectionString,
                  tenantsArray[0]
                );
              }
              else if (tenantsArray.Length > 0)
              {
                databases
                  .AddMultipleTenantDatabase(connectionString)
                  .ForTenants(tenants.ToArray());
              }
            }
          });

          if (Environment.IsDevelopment())
          {
            options.AutoCreateSchemaObjects = AutoCreate.All;
          }
        }
      )
      .AssertDatabaseMatchesConfigurationOnStartup()
      .OptimizeArtifactWorkflow();

    services.AddScoped<EventStoreSession>();
    services.AddScoped<EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();
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
