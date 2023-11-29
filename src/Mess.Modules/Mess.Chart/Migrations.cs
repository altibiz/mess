using Etch.OrchardCore.Fields.Colour.Settings;
using Mess.Chart.Indexes;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using YesSql.Sql;

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
          .WithDisplayName("Chart")
          .WithDescription(
            "Provides a chart or dashboard for your content item."
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "TimeseriesChartPart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Timeseries chart")
          .WithField(
            "History",
            builder =>
              builder
                .OfType("IntervalField")
                .WithDisplayName("History")
                .WithDescription("The history for the chart.")
                .WithPosition("0")
          )
          .WithField(
            "RefreshInterval",
            builder =>
              builder
                .OfType("IntervalField")
                .WithDisplayName("Refresh interval")
                .WithDescription("The refresh interval for the chart.")
                .WithPosition("1")
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "TimeseriesChartDatasetPart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Line chart dataset")
          .WithDescription("Provides a line chart dataset for your line chart.")
          .WithField(
            "Label",
            builder =>
              builder
                .OfType("TextField")
                .WithDisplayName("Label")
                .WithDescription("The label for the dataset.")
                .WithSettings<TextFieldSettings>(
                  new() { Hint = "The label for the dataset.", Required = true }
                )
                .WithPosition("0")
          )
          .WithField(
            "Color",
            builder =>
              builder
                .OfType("ColourField")
                .WithDisplayName("Color")
                .WithDescription("The color for the line of the dataset.")
                .WithSettings<ColourFieldSettings>(
                  new()
                  {
                    Hint = "The color for the line of the dataset.",
                    DefaultValue = "#0000FF",
                    Colours = new ColourItem[]
                    {
                      new() { Name = "Red", Value = "#FF0000" },
                      new() { Name = "Green", Value = "#00FF00" },
                      new() { Name = "Blue", Value = "#0000FF" },
                      new() { Name = "Cyan", Value = "#00FFFF" },
                      new() { Name = "Magenta", Value = "#FF00FF" },
                      new() { Name = "Yellow", Value = "#FFFF00" }
                    }
                  }
                )
                .WithPosition("1")
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "TimeseriesChart",
      builder =>
        builder
          .Stereotype("Chart")
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Timeseries chart")
          .WithDescription("A timeseries line chart.")
          .WithPart(
            "TitlePart",
            builder =>
              builder
                .WithDisplayName("Title")
                .WithSettings<TitlePartSettings>(
                  new() { Options = TitlePartOptions.EditableRequired, }
                )
                .WithPosition("0")
          )
          .WithPart(
            "TimeseriesChartPart",
            builder =>
              builder.WithDisplayName("Timeseries chart").WithPosition("1")
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "TimeseriesChartDataset",
      builder =>
        builder.WithPart(
          "TimeseriesChartDatasetPart",
          builder => builder.WithPosition("0")
        )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "Dashboard",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Dashboard")
          .WithDescription("A dashboard with multiple charts.")
          .WithPart(
            "TitlePart",
            builder =>
              builder
                .WithDisplayName("Title")
                .WithSettings<TitlePartSettings>(
                  new() { Options = TitlePartOptions.EditableRequired, }
                )
                .WithPosition("0")
          )
          .WithPart(
            "FlowPart",
            builder =>
              builder
                .WithDisplayName("Charts")
                .WithSettings<FlowPartSettings>(
                  new() { ContainedContentTypes = new[] { "TimeseriesChart" }, }
                )
                .WithPosition("1")
          )
    );

    SchemaBuilder.CreateMapIndexTable<ChartIndex>(
      table =>
        table
          .Column<string>("ChartContentItemId", c => c.WithLength(30))
          .Column<string>("ContentType", c => c.WithLength(30))
          .Column<string>("Title", c => c.WithLength(200))
    );
    SchemaBuilder.AlterIndexTable<ChartIndex>(
      table => table.CreateIndex("IDX_ChartIndex_ContentType", "ContentType")
    );

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
}
