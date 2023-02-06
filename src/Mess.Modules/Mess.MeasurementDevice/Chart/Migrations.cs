using Mess.MeasurementDevice.Chart.Providers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace Mess.MeasurementDevice.Chart;

public class Migrations : DataMigration
{
  public int Create()
  {
    _contentDefinitionManager.AlterTypeDefinition(
      "EgaugeMeasurementDevice",
      builder =>
        builder
          .DisplayedAs("Egauge measurement device")
          .WithDescription("An Egauge measurement device.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the Egauge measurement device."
                )
                .WithPosition("1")
                .WithSettings(
                  new
                  {
                    RenderTitle = true,
                    Options = 1, // NOTE: GeneratedDisabled
                    Pattern = @"{%- ContentItem.Content.MeasurementDevicePart.DeviceId -%}"
                  }
                )
          )
          .WithPart(
            "MeasurementDevicePart",
            part =>
              part.WithDisplayName("Device")
                .WithPosition("2")
                .WithDescription("Neccessary device data.")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Line chart displaying measurements in the last 24 hours."
                )
                .WithPosition("3")
                .WithSettings(new { Provider = EgaugeChartProvider.ProviderId })
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
