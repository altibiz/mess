using Mess.MeasurementDevice.Chart;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace Mess.MeasurementDevice;

public class Migrations : DataMigration
{
  public int Create()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "MeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription(
            "Provides neccessary fields for every measurement device."
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "EgaugeMeasurementDevice",
      builder =>
        builder
          .Creatable()
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
                .WithDescription("Neccessary device data.")
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
