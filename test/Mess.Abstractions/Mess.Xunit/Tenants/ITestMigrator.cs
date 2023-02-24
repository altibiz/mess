using Mess.Tenants;

namespace Mess.Xunit.Tenants;

public interface ITestMigrator
{
  public void Migrate(ITenants tenants) =>
    MigrateAsync(tenants).RunSynchronously();

  public Task MigrateAsync(ITenants tenants);
}
