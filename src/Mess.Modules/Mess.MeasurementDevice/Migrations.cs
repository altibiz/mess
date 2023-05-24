using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;

namespace Mess.MeasurementDevice;

public class Migrations : DataMigration
{
  public int Create()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "EgaugeMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An Egauge measurement device.")
          .WithDisplayName("Egauge measurement device")
          .WithField(
            "DeviceId",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Device identifier")
                .WithDescription(
                  "The identifier of the Egauge measurement device."
                )
                .WithSettings(
                  new TextFieldSettings
                  {
                    Hint = "The identifier of the Egauge measurement device."
                  }
                )
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "EgaugeMeasurementDevice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
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
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.EgaugeMeasurementDevicePart.DeviceId.Text -%}"
                  }
                )
          )
          .WithPart(
            "EgaugeMeasurementDevicePart",
            part =>
              part.WithDisplayName("Egauge measurement device")
                .WithDescription("An Egauge measurement device.")
                .WithPosition("2")
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
