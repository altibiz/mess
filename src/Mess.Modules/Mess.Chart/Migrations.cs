using Mess.Chart.Fields;
using Mess.Chart.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace Mess.Chart;

public class Migrations : DataMigration
{
  public int Create()
  {
    _contentDefinitionManager.MigrateFieldSettings<
      ChartField,
      ChartFieldSettings
    >();

    _contentDefinitionManager.AlterPartDefinition(
      "Chart",
      builder =>
        builder
          .Attachable()
          .WithDescription("Provides a chart for your content item.")
    );

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
}
