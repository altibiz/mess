using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Mess.Util.Extensions.OrchardCore;

namespace Mess.Timeseries.Migrations.OrchardCore.M0;

public static partial class RecipeMigrations
{
  public static IRecipeMigrator ExecuteTimeseriesMigration(
    this IRecipeMigrator recipe,
    IDataMigration migration
  ) =>
    recipe.Execute("OrchardCore/M0/MigrateTimeseries.recipe.json", migration);
}
