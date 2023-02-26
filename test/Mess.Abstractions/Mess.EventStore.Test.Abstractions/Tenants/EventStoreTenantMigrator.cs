using Mess.Tenants;
using Mess.Test.Tenants;

namespace Mess.EventStore.Test.Abstractions.Tenants;

public class EventStoreTestTenantMigrator : ITestTenantMigrator
{
  public void Migrate(ITenants tenants) { }

  public Task MigrateAsync(ITenants tenants)
  {
    return Task.CompletedTask;
  }
}
