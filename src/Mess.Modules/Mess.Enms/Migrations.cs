using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;

namespace Mess.Enms;

public class Migrations : DataMigration
{
  private readonly IContentDefinitionManager _contentDefinitionManager;

  public Migrations(
    IContentDefinitionManager contentDefinitionManager
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
  }

  public int Create()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "EgaugeIotDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An Egauge measurement device.")
          .WithDisplayName("Egauge measurement device")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "EgaugeIotDevice",
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{{ ContentItem.Content.EgaugeIotDevicePart.DeviceId.Text }}"
                  }
                )
          )
          .WithPart(
            "IotDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
                .WithPosition("2")
          )
          .WithPart(
            "EgaugeIotDevicePart",
            part =>
              part.WithDisplayName("Egauge measurement device")
                .WithDescription("An Egauge measurement device.")
                .WithPosition("3")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Egauge measurement device data."
                )
                .WithPosition("4")
          )
    );

    return 1;
  }
}
