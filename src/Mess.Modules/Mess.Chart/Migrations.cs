using Etch.OrchardCore.Fields.Colour.Settings;
using Etch.OrchardCore.Fields.MultiSelect.Settings;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;

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
          .WithDescription("Provides a chart for your content item.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LineChartPart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Line chart")
          .WithDescription("Provides a line chart for your chart.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LineChartDatasetPart",
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
                    // TODO: configurable through theme?
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

    _contentDefinitionManager.AlterPartDefinition(
      "TimeseriesChartDatasetPart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Timeseries chart dataset")
          .WithDescription(
            "Provides a timeseries property for your line chart dataset."
          )
          .WithField(
            "History",
            builder =>
              builder
                .OfType("MultiSelectField")
                .WithDisplayName("History")
                .WithDescription("The history for the dataset.")
                .WithSettings<MultiSelectFieldSettings>(
                  new()
                  {
                    Hint = "The history for the dataset. (default is 1 hour)",
                    Options = new[] { "Hour", "Day", "Week", "Month", "Year" },
                  }
                )
                .WithPosition("0")
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "Chart",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Versionable()
          .DisplayedAs("Chart")
          .WithDescription("A chart.")
          .WithPart(
            "TitlePart",
            builder =>
              builder
                .WithDisplayName("Title")
                .WithDescription("Title of the chart.")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    Options = TitlePartOptions.EditableRequired,
                    RenderTitle = true,
                  }
                )
                .WithPosition("0")
          )
          .WithPart(
            "ChartPart",
            builder =>
              builder
                .WithDisplayName("Chart")
                .WithDescription("Chart content.")
                .WithPosition("1")
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "LineChart",
      builder =>
        builder
          .Stereotype("ConcreteChart")
          .DisplayedAs("Line chart")
          .WithDescription("A line chart.")
          .WithPart(
            "LineChartPart",
            builder =>
              builder
                .WithPosition("0")
                .WithDisplayName("Line chart")
                .WithDescription("Line chart content.")
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "LineChartDataset",
      builder =>
        builder
          .DisplayedAs("Line chart dataset")
          .WithDescription("A line chart dataset.")
          .WithPart(
            "LineChartDatasetPart",
            builder =>
              builder
                .WithPosition("0")
                .WithDisplayName("Line chart dataset")
                .WithDescription("Line chart dataset content.")
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "TimeseriesChartDataset",
      builder =>
        builder
          .Stereotype("ConcreteLineChartDataset")
          .DisplayedAs("Timeseries chart dataset")
          .WithDescription("A timeseries line chart dataset.")
          .WithPart(
            "TimeseriesChartDatasetPart",
            builder =>
              builder
                .WithPosition("0")
                .WithDisplayName("Timeseries chart dataset")
                .WithDescription("Timeseries chart dataset content.")
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
