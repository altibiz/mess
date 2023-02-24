using Mess.Tenants;
using Mess.Timeseries.Abstractions.Client;
using Mess.Xunit.Tenants;

namespace Mess.Timeseries.Test.Abstractions.Tenants;

public class TimeseriesTestMigrator : ITestMigrator
{
  public void Migrate(ITenants tenants)
  {
    _migrator.Migrate();
  }

  public async Task MigrateAsync(ITenants tenants)
  {
    await _migrator.MigrateAsync();
  }

  public TimeseriesTestMigrator(ITimeseriesMigrator migrator)
  {
    _migrator = migrator;
  }

  private readonly ITimeseriesMigrator _migrator;
}
