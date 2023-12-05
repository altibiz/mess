using Etch.OrchardCore.Fields.Colour.Fields;
using Mess.Chart.Abstractions.Models;
using Mess.Cms;
using Mess.Enms.Abstractions.Models;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Fields.Abstractions;
using Mess.Fields.Abstractions.Fields;
using Mess.Population.Abstractions;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Enms;

public class Populations : IPopulation
{
  private readonly IContentManager _contentManager;

  public Populations(IContentManager contentManager)
  {
    _contentManager = contentManager;
  }

  public async Task PopulateAsync()
  {
    var eguagePowerDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    eguagePowerDataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power" };
        timeseriesChartDatasetPart.Property = nameof(EgaugeMeasurement.Power);
      }
    );
    var egaugeChart =
      await _contentManager.NewContentAsync<TimeseriesChartItem>();
    egaugeChart.Alter(
      egaugeChart => egaugeChart.TitlePart,
      titlePart => { titlePart.Title = "Egauge"; }
    );
    egaugeChart.Inner.DisplayText = "Egauge";
    egaugeChart.Alter(
      egaugeChart => egaugeChart.TimeseriesChartPart,
      timeseriesChartPart =>
      {
        timeseriesChartPart.ChartContentType = "EgaugeIotDevice";
        timeseriesChartPart.History = new IntervalField
        {
          Value = new Interval(IntervalUnit.Minute, 10)
        };
        timeseriesChartPart.RefreshInterval = new IntervalField
        {
          Value = new Interval(IntervalUnit.Second, 10)
        };
        timeseriesChartPart.Datasets = new List<ContentItem>
          { eguagePowerDataset };
      }
    );
    await _contentManager.CreateAsync(egaugeChart, VersionOptions.Latest);

    var egaugeIotDevice =
      await _contentManager.NewContentAsync<EgaugeIotDeviceItem>();
    egaugeIotDevice.Alter(
      egaugeIotDevice => egaugeIotDevice.TitlePart,
      titlePart => { titlePart.Title = "Egauge"; }
    );
    egaugeIotDevice.Inner.DisplayText = "Egauge";
    egaugeIotDevice.Alter(
      egaugeIotDevice => egaugeIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new TextField { Text = "egauge" };
      }
    );
    egaugeIotDevice.Alter(
      egaugeIotDevice => egaugeIotDevice.ChartPart,
      chartPart => { chartPart.ChartContentItemId = egaugeChart.ContentItemId; }
    );
    await _contentManager.CreateAsync(egaugeIotDevice, VersionOptions.Latest);
  }
}
