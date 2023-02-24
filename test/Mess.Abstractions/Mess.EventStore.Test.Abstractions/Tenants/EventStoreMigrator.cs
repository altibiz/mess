using Mess.Tenants;
using Mess.Xunit.Tenants;

namespace Mess.EventStore.Test.Abstractions.Tenants;

public class EventStoreTestMigrator : ITestMigrator
{
  public void Migrate(ITenants tenants) { }

  public Task MigrateAsync(ITenants tenants)
  {
    return Task.CompletedTask;
  }
}
