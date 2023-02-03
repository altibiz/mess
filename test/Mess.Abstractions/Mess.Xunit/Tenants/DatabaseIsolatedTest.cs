using Mess.Tenants;

namespace Mess.Xunit.Tenants;

public record class DatabaseIsolatedTest
{
  public DatabaseIsolatedTest(IServiceProvider services)
  {
    using var scope = services.CreateScope();

    var tenants = scope.ServiceProvider.GetRequiredService<ITenants>();
    var testMigrators = scope.ServiceProvider.GetServices<ITestMigrator>();

    foreach (var testMigrator in testMigrators)
    {
      testMigrator.Migrate(tenants);
    }
  }
}
