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
        builder.WithDescription("Provides a chart for your content item.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LineChartPart",
      builder =>
        builder.WithDescription("Provides a line chart for your chart.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LineChartDatasetPart",
      builder =>
        builder.WithDescription(
          "Provides a line chart dataset for your line chart."
        )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "TimeseriesChartDatasetPart",
      builder =>
        builder.WithDescription(
          "Provides a timeseries property for your line chart dataset."
        )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "Chart",
      builder =>
        builder.Creatable().WithDescription("A chart.").WithPart("ChartPart")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "LineChart",
      builder =>
        builder
          .Stereotype("ConcreteChart")
          .WithDescription("A line chart.")
          .WithPart("LineChartPart")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "LineChartDataset",
      builder =>
        builder
          .WithDescription("A line chart dataset.")
          .WithPart("LineChartDatasetPart")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "TimeseriesChartDataset",
      builder =>
        builder
          .Stereotype("ConcreteLineChartDataset")
          .WithDescription("A timeseries line chart dataset.")
          .WithPart("TimeseriesChartDatasetPart")
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
