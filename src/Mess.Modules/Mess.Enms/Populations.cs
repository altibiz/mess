using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;
using Mess.Enms.Abstractions.Models;
using Mess.Chart.Abstractions.Models;
using Mess.Fields.Abstractions;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Population.Abstractions;

namespace Mess.Enms;

public class Populations : IPopulation
{
  public async Task PopulateAsync()
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
        timeseriesChartPart.ChartContentType = "EgaugeIotDevice";
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

    var egaugeIotDevice =
      await _contentManager.NewContentAsync<EgaugeIotDeviceItem>();
    egaugeIotDevice.Alter(
      egaugeIotDevice => egaugeIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new() { Text = "egauge" };
      }
    );
    egaugeIotDevice.Alter(
      egaugeIotDevice => egaugeIotDevice.ChartPart,
      chartPart =>
      {
        chartPart.ChartContentItemId = egaugeChart.ContentItemId;
      }
    );
    await _contentManager.CreateAsync(egaugeIotDevice, VersionOptions.Latest);
  }

  public Populations(IContentManager contentManager)
  {
    _contentManager = contentManager;
  }

  private readonly IContentManager _contentManager;
}
