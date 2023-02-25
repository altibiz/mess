using Mess.Tenants;
using Mess.Timeseries.Abstractions.Client;
using Mess.Test.Tenants;

namespace Mess.Timeseries.Test.Abstractions.Tenants;

public class TimeseriesTestTenantMigrator : ITestTenantMigrator
{
  public void Migrate(ITenants tenants)
  {
    _migrator.Migrate();
  }

  public async Task MigrateAsync(ITenants tenants)
  {
    await _migrator.MigrateAsync();
  }

  public TimeseriesTestTenantMigrator(ITimeseriesMigrator migrator)
  {
    _migrator = migrator;
  }

  private readonly ITimeseriesMigrator _migrator;
}
