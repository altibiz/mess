namespace Mess.Timeseries.Client;

public interface ITimeseriesMigrator
{
  public void Migrate();

  public Task MigrateAsync();
}
