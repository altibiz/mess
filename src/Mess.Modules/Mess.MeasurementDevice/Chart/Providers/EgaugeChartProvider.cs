using Mess.Tenants;
using Mess.MeasurementDevice.Abstractions.Client;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Models;
using Mess.MeasurementDevice.Models;
using Mess.Chart.Abstractions.Extensions.System;
using Mess.Timeseries.Abstractions.Entities;
using Mess.System.Extensions.IEnumerable;

namespace Mess.MeasurementDevice.Chart.Providers;

public class EgaugeChartDataProvider : IChartDataProvider
{
  public const string ProviderId = "Egauge";

  public string Id => ProviderId;

  public ChartModel? CreateChart(ContentItem contentItem)
  {
    var chartPart = contentItem.As<ChartPart>();
    if (chartPart is null)
    {
      return null;
    }

    var lineChart = chartPart.Chart?.As<LineChartPart>();
    if (lineChart is null)
    {
      return null;
    }

    var measurementDevice = contentItem.As<MeasurementDevicePart>();
    if (measurementDevice is null)
    {
      return null;
    }
    var now = DateTime.UtcNow;
    var contentItemDatasets = lineChart.Datasets;

    var measurements = _client.GetEgaugeMeasurements(
      source: measurementDevice.DeviceId.Text,
      beginning: now.Subtract(TimeSpan.FromDays(1)),
      end: now
    );

    var datasets = lineChart.Datasets
      .Select(contentItem => contentItem.As<LineChartDatasetPart>())
      .Select(dataset =>
      {
        if (dataset is null)
        {
          return null;
        }

        var timesereiesDataset =
          dataset.Dataset.As<TimeseriesChartDatasetPart>();
        if (timesereiesDataset is null)
        {
          return null;
        }

        return measurements.ToTimeseriesChartDataset(
            label: dataset.Label,
            color: dataset.Color,
            history: timesereiesDataset.History,
            xField: nameof(HypertableEntity.Timestamp),
            yField: timesereiesDataset.Property
          ) as LineChartDatasetModel;
      })
      .WhereNotNull()
      .ToList();

    return new LineChartModel(datasets);
  }

  public async Task<ChartModel?> CreateChartAsync(ContentItem contentItem)
  {
    var chartPart = contentItem.As<ChartPart>();
    if (chartPart is null)
    {
      return null;
    }

    var lineChart = chartPart.Chart?.As<LineChartPart>();
    if (lineChart is null)
    {
      return null;
    }

    var measurementDevice = contentItem.As<MeasurementDevicePart>();
    if (measurementDevice is null)
    {
      return null;
    }
    var now = DateTime.UtcNow;
    var contentItemDatasets = lineChart.Datasets;

    var measurements = await _client.GetEgaugeMeasurementsAsync(
      source: measurementDevice.DeviceId.Text,
      beginning: now.Subtract(TimeSpan.FromDays(1)),
      end: now
    );

    var datasets = lineChart.Datasets
      .Select(contentItem => contentItem.As<LineChartDatasetPart>())
      .Where(dataset => dataset is not null)
      .Select(dataset =>
      {
        var timesereiesDataset =
          dataset.Dataset.As<TimeseriesChartDatasetPart>();
        if (timesereiesDataset is null)
        {
          return null;
        }

        return measurements.ToTimeseriesChartDataset(
            label: dataset.Label,
            color: dataset.Color,
            history: timesereiesDataset.History,
            xField: nameof(HypertableEntity.Timestamp),
            yField: timesereiesDataset.Property
          ) as LineChartDatasetModel;
      })
      .WhereNotNull()
      .ToList();

    return new LineChartModel(datasets);
  }

  public EgaugeChartDataProvider(ITenants tenants, IMeasurementClient client)
  {
    _tenants = tenants;
    _client = client;
  }

  private readonly ITenants _tenants;
  private readonly IMeasurementClient _client;
}
