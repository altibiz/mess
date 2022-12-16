using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace Mess.Util.Extensions.OrchardCore;

public static class IRecipeMigratorExtensions
{
  public static IRecipeMigrator Execute(
    this IRecipeMigrator schema,
    string recipe,
    IDataMigration migration
  )
  {
    var task = schema.ExecuteAsync(recipe, migration);
    task.Wait();
    return schema;
  }
}
