using Mess.Prelude.Test.Migrations;
using Mess.Relational.Abstractions.Migrations;

namespace Mess.Relational.Test.Abstractions.Migrations;

public class RelationalDbTestMigrator : ITestMigrator
{
  private readonly IRelationalDbMigrator _migrator;

  public RelationalDbTestMigrator(IRelationalDbMigrator migrator)
  {
    _migrator = migrator;
  }

  public async Task MigrateAsync()
  {
    await _migrator.MigrateAsync();
  }
}
