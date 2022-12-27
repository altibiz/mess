using Microsoft.Extensions.DependencyInjection;
using Marten;
using Weasel.Core;
using System.Reflection;

namespace Mess.EventStore.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddMartenFromTenantGroups(
    this IServiceCollection services,
    bool isDevelopment,
    IDictionary<string, IEnumerable<string>> configuration
  )
  {
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

          if (isDevelopment)
          {
            options.AutoCreateSchemaObjects = AutoCreate.All;
          }

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
        }
      )
      .AssertDatabaseMatchesConfigurationOnStartup()
      .OptimizeArtifactWorkflow();
  }
}
