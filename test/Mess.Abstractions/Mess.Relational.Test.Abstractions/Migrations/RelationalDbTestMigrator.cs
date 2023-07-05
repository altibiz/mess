using Mess.System.Test.Migrations;
using Mess.Relational.Abstractions.Migrations;

namespace Mess.Relational.Test.Abstractions.Migrations;

public class RelationalDbTestMigrator : ITestMigrator
{
  public async Task MigrateAsync()
  {
    await _migrator.MigrateAsync();
  }

  public RelationalDbTestMigrator(IRelationalDbMigrator migrator)
  {
    _migrator = migrator;
  }

  private readonly IRelationalDbMigrator _migrator;
}
