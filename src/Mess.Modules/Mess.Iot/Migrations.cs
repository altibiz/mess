using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Models;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using Mess.OrchardCore;
using YesSql.Sql;
using Microsoft.Extensions.Hosting;
using Mess.Fields.Abstractions.Services;

namespace Mess.Iot;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "MeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A measurement device.")
          .WithDisplayName("Measurement device")
          .WithField(
            "DeviceId",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Device identifier")
                .WithDescription("The identifier of the measurement device.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Hint = "The identifier of the measurement device."
                  }
                )
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "EgaugeMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An Egauge measurement device.")
          .WithDisplayName("Egauge measurement device")
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
                      @"{%- ContentItem.Content.MeasurementDevicePart.DeviceId.Text -%}"
                  }
                )
          )
          .WithPart(
            "MeasurementDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
                .WithPosition("2")
          )
          .WithPart(
            "EgaugeMeasurementDevicePart",
            part =>
              part.WithDisplayName("Egauge measurement device")
                .WithDescription("An Egauge measurement device.")
                .WithPosition("3")
          )
    );

    SchemaBuilder.CreateMapIndexTable<MeasurementDeviceIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<bool>("IsMessenger")
    );
    SchemaBuilder.AlterIndexTable<MeasurementDeviceIndex>(
      table =>
        table.CreateIndex("IDX_MeasurementDeviceIndex_DeviceId", "DeviceId")
    );

    if (_hostEnvironment.IsDevelopment())
    {
      var egaugeMeasurementDevice =
        await _contentManager.NewContentAsync<EgaugeMeasurementDeviceItem>();
      egaugeMeasurementDevice.Alter(
        egaugeMeasurementDevice =>
          egaugeMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "egauge" };
        }
      );
      await _contentManager.CreateAsync(
        egaugeMeasurementDevice,
        VersionOptions.Latest
      );
    }

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager,
    IHostEnvironment hostEnvironment,
    IRecipeMigrator recipeMigrator,
    IApiKeyFieldService apiKeyFieldService
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _contentManager = contentManager;
    _recipeMigrator = recipeMigrator;
    _hostEnvironment = hostEnvironment;
    _apiKeyFieldService = apiKeyFieldService;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IContentManager _contentManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly IHostEnvironment _hostEnvironment;
  private readonly IApiKeyFieldService _apiKeyFieldService;
}