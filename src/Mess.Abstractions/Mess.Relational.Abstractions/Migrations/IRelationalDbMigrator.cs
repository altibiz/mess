namespace Mess.Relational.Abstractions.Migrations;

public interface IRelationalDbMigrator
{
  public Task MigrateAsync();
}
