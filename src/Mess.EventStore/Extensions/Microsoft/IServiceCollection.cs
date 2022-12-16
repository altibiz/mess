using Microsoft.Extensions.DependencyInjection;
using Marten;
using Weasel.Core;

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
        }
      )
      .AssertDatabaseMatchesConfigurationOnStartup()
      .OptimizeArtifactWorkflow();
  }
}
