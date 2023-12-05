using Mess.Relational.Abstractions.Migrations;
using OrchardCore.Modules;

namespace Mess.Relational;

public class RelationalDbMigrationModularTenantEvents : ModularTenantEvents
{
  private readonly List<IRelationalDbMigrator> _relationalDbMigrators;

  public RelationalDbMigrationModularTenantEvents(
    IEnumerable<IRelationalDbMigrator> relationalDbMigrators
  )
  {
    _relationalDbMigrators = relationalDbMigrators.ToList();
  }

  public override async Task ActivatingAsync()
  {
    foreach (var relationalDbMigrator in _relationalDbMigrators)
      await relationalDbMigrator.MigrateAsync();
  }
}
