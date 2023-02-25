using Mess.Tenants;

namespace Mess.Test.Tenants;

public interface ITestTenantMigrator
{
  public void Migrate(ITenants tenants) =>
    MigrateAsync(tenants).RunSynchronously();

  public Task MigrateAsync(ITenants tenants);
}
