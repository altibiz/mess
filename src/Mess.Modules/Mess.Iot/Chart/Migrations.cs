using Mess.Chart.Abstractions.Models;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using Mess.OrchardCore;
using Mess.Iot.Abstractions.Models;
using Mess.Iot.Abstractions.Client;
using YesSql;
using Mess.Iot.Abstractions.Indexes;
using Mess.Fields.Abstractions;

namespace Mess.Iot.Chart;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
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

    if (_hostEnvironment.IsDevelopment())
    {
      var eguagePowerDataset =
        await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
      eguagePowerDataset.Alter(
        eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
        timeseriesChartDatasetPart =>
        {
          timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
          timeseriesChartDatasetPart.Label = new() { Text = "Power" };
          timeseriesChartDatasetPart.Property = nameof(EgaugeMeasurement.Power);
        }
      );
      var egaugeChart =
        await _contentManager.NewContentAsync<TimeseriesChartItem>();
      egaugeChart.Alter(
        egaugeChart => egaugeChart.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Egauge";
        }
      );
      egaugeChart.Alter(
        egaugeChart => egaugeChart.TimeseriesChartPart,
        timeseriesChartPart =>
        {
          timeseriesChartPart.ChartContentType = "EgaugeMeasurementDevice";
          timeseriesChartPart.History = new()
          {
            Value = new(Unit: IntervalUnit.Minute, Count: 10)
          };
          timeseriesChartPart.RefreshInterval = new()
          {
            Value = new(Unit: IntervalUnit.Second, Count: 10)
          };
          timeseriesChartPart.Datasets = new() { eguagePowerDataset };
        }
      );
      await _contentManager.CreateAsync(egaugeChart, VersionOptions.Latest);

      var egaugeMeasurementDevice =
        (
          await _session
            .Query<ContentItem, MeasurementDeviceIndex>()
            .Where(index => index.DeviceId == "egauge")
            .FirstOrDefaultAsync()
        )?.AsContent<EgaugeMeasurementDeviceItem>()
        ?? await _contentManager.NewContentAsync<EgaugeMeasurementDeviceItem>();
      egaugeMeasurementDevice.Alter(
        egaugeMeasurementDevice =>
          egaugeMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "egauge" };
        }
      );
      egaugeMeasurementDevice.Alter(
        egaugeMeasurementDevice => egaugeMeasurementDevice.ChartPart,
        chartPart =>
        {
          chartPart.ChartContentItemId = egaugeChart.ContentItemId;
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
