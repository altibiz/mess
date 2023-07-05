namespace Mess.System.Test.Migrations;

public interface ITestMigrator
{
  public Task MigrateAsync();
}
