using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace Mess.Chart;

public class Migrations : DataMigration
{
  public int Create()
  {
    return 1;
  }

  public Migrations(IContentDefinitionManager content, IRecipeMigrator recipe)
  {
    Content = content;
    Recipe = recipe;
  }

  private IContentDefinitionManager Content { get; }
  private IRecipeMigrator Recipe { get; }
}
