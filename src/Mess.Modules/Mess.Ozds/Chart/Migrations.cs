using Mess.Chart.Abstractions.Models;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using YesSql;
using Mess.Iot.Abstractions.Indexes;
using Mess.ContentFields.Abstractions;
using Mess.Ozds.Abstractions.Client;

namespace Mess.Iot.Chart;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "AbbMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A Abb measurement device.")
          .WithDisplayName("Abb measurement device")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "AbbMeasurementDevice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Abb measurement device")
          .WithDescription("An Abb measurement device.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the Abb measurement device."
                )
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.AbbMeasurementDevicePart.DeviceId.Text -%}"
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
            "AbbMeasurementDevicePart",
            part =>
              part.WithDisplayName("Abb measurement device")
                .WithDescription("An Abb measurement device.")
                .WithPosition("3")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Abb measurement device data."
                )
                .WithPosition("4")
          )
    );

    if (_hostEnvironment.IsDevelopment())
    {
      var abbPowerDataset =
        await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
      abbPowerDataset.Alter(
        eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
        timeseriesChartDatasetPart =>
        {
          timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
          timeseriesChartDatasetPart.Label = new() { Text = "Power" };
          timeseriesChartDatasetPart.Property = nameof(
            AbbMeasurement.ActivePowerL1
          );
        }
      );
      var abbChart =
        await _contentManager.NewContentAsync<TimeseriesChartItem>();
      abbChart.Alter(
        abbChart => abbChart.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Abb";
        }
      );
      abbChart.Alter(
        abbChart => abbChart.TimeseriesChartPart,
        timeseriesChartPart =>
        {
          timeseriesChartPart.ChartContentType = "AbbMeasurementDevice";
          timeseriesChartPart.History = new()
          {
            Value = new(Unit: IntervalUnit.Minute, Count: 10)
          };
          timeseriesChartPart.RefreshInterval = new()
          {
            Value = new(Unit: IntervalUnit.Second, Count: 10)
          };
          timeseriesChartPart.Datasets = new() { abbPowerDataset };
        }
      );
      await _contentManager.CreateAsync(abbChart, VersionOptions.Latest);

      var abbMeasurementDevice =
        (
          await _session
            .Query<ContentItem, MeasurementDeviceIndex>()
            .Where(index => index.DeviceId == "abb")
            .FirstOrDefaultAsync()
        )?.AsContent<AbbMeasurementDeviceItem>()
        ?? await _contentManager.NewContentAsync<AbbMeasurementDeviceItem>();
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "abb" };
        }
      );
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.ChartPart,
        chartPart =>
        {
          chartPart.ChartContentItemId = abbChart.ContentItemId;
        }
      );
      await _contentManager.CreateAsync(
        abbMeasurementDevice,
        VersionOptions.Latest
      );
    }

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator,
    IHostEnvironment hostEnvironment,
    IContentManager contentManager,
    ISession session
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _hostEnvironment = hostEnvironment;
    _contentManager = contentManager;
    _session = session;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly IContentManager _contentManager;
  private readonly IHostEnvironment _hostEnvironment;
  private readonly ISession _session;
}
