namespace Mess.Timeseries.Abstractions.Client;

public interface ITimeseriesMigrator
{
  public void Migrate();

  public Task MigrateAsync();
}
