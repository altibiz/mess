using Microsoft.Extensions.DependencyInjection;
using Marten;
using Weasel.Core;
using System.Reflection;
using Mess.EventStore.Services;
using Mess.Tenants;

namespace Mess.EventStore.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddMartenFromTenantGroups(
    this IServiceCollection services,
    bool isDevelopment,
    IReadOnlyDictionary<string, IReadOnlyList<Tenant>> configuration
  )
  {
    var projection = new Projection();
    services.AddSingleton<Projection>(projection);

    services
      .AddMarten(
        (StoreOptions options) =>
        {
          options.MultiTenantedDatabases(databases =>
          {
            foreach (var (connectionString, tenants) in configuration)
            {
              var tenantsArray = tenants.ToArray();
              if (tenantsArray.Length == 1)
              {
                databases.AddSingleTenantDatabase(
                  connectionString,
                  tenantsArray[0].Name
                );
              }
              else if (tenantsArray.Length > 0)
              {
                databases
                  .AddMultipleTenantDatabase(connectionString)
                  .ForTenants(tenants.Select(tenant => tenant.Name).ToArray());
              }
            }
          });

          options.AutoCreateSchemaObjects = AutoCreate.None;
          foreach (
            var eventType in Assembly
              .GetExecutingAssembly()
              .GetTypes()
              .Where(
                type =>
                  type.Namespace == "Mess.EventStore.Events"
                  && type.IsClass
                  && !type.IsAbstract
                  && type.IsPublic
              )
          )
          {
            options.Events.AddEventType(eventType);
          }

          options.Projections.Add(
            projection,
            global::Marten.Events.Projections.ProjectionLifecycle.Inline
          );
        }
      )
      // NOTE: doesn't start?
      // .AddAsyncDaemon(
      //   global::Marten.Events.Daemon.Resiliency.DaemonMode.HotCold
      // )
      .AssertDatabaseMatchesConfigurationOnStartup()
      .OptimizeArtifactWorkflow();
  }
}
