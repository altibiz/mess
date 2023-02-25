using Mess.Tenants;

namespace Mess.Test.Tenants;

public record class TenantFixture : ITenantFixture
{
  public ITenants Tenants { get; }

  public TenantFixture(IServiceProvider services)
  {
    var tenants = services.GetRequiredService<ITenants>();
    var testMigrators = services.GetServices<ITestTenantMigrator>();

    foreach (var testMigrator in testMigrators)
    {
      testMigrator.Migrate(tenants);
    }

    Tenants = tenants;
  }
}
