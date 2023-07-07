using Mess.Relational.Abstractions.Migrations;
using OrchardCore.Modules;

namespace Mess.Relational.Migrations;

public class RelationalDbMigrationModularTenantEvents : ModularTenantEvents
{
  public override async Task ActivatingAsync()
  {
    foreach (var relationalDbMigrator in _relationalDbMigrators)
    {
      await relationalDbMigrator.MigrateAsync();
    }
  }

  public RelationalDbMigrationModularTenantEvents(
    IEnumerable<IRelationalDbMigrator> relationalDbMigrators
  )
    : base()
  {
    _relationalDbMigrators = relationalDbMigrators.ToList();
  }

  private readonly List<IRelationalDbMigrator> _relationalDbMigrators;
}
