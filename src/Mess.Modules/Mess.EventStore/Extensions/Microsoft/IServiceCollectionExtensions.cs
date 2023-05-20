using Microsoft.Extensions.DependencyInjection;
using Marten;
using Weasel.Core;
using System.Reflection;
using Mess.EventStore.Services;
using Mess.Tenants;
using Marten.Events.Projections;

// using Marten.Events.Daemon.Resiliency;

namespace Mess.EventStore.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddMartenFromTenantGroups(
    this IServiceCollection services,
    bool isDevelopment,
    IReadOnlyDictionary<string, IReadOnlyList<Tenant>> configuration
  )
  {
    services
      .AddMarten(
        (IServiceProvider services) =>
        {
          var options = new StoreOptions();

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

          var projection = new Projection(services);
          options.Projections.Add(projection, ProjectionLifecycle.Inline);

          return options;
        }
      )
      // NOTE: doesn't start?
      // .AddAsyncDaemon(DaemonMode.HotCold)
      .AssertDatabaseMatchesConfigurationOnStartup()
      .OptimizeArtifactWorkflow();
  }
}
