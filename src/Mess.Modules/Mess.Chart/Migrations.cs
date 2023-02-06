using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace Mess.Chart;

public class Migrations : DataMigration
{
  public int Create()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "ChartPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("Provides a chart for your content item.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LineChartPar",
      builder =>
        builder
          .Attachable()
          .WithDescription("Provides a line chart for your chart.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LineChartDatasetPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("Provides a line chart dataset for your line chart.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "TimeseriesChartDatasetPart",
      builder =>
        builder
          .Attachable()
          .WithDescription(
            "Provides a timeseries property for your line chart dataset."
          )
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
