using Marten;
using Mess.EventStore.Test.Abstractions.Tenants;
using Mess.Tenants;
using Mess.Xunit.Tenants;

namespace Mess.EventStore.Test.Abstractions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection RegisterTestEventStore(
    this IServiceCollection services
  )
  {
    // NOTE: this is meant to be a singleton but here we want a scoped service
    // because of tenants
    services.AddScoped<IDocumentStore>(services =>
    {
      var tenant = services.GetRequiredService<ITenants>();
      return DocumentStore.For(options =>
      {
        options.MultiTenantedDatabases(databases =>
        {
          databases.AddSingleTenantDatabase(
            tenant.Current.ConnectionString,
            tenant.Current.Name
          );
        });
      });
    });
    services.AddScoped<IDocumentSession>(
      services => services.GetRequiredService<IDocumentStore>().OpenSession()
    );
    services.AddScoped<IQuerySession>(
      services => services.GetRequiredService<IDocumentStore>().QuerySession()
    );
    services.AddScoped<ITestMigrator, EventStoreTestMigrator>();
    return services;
  }
}
