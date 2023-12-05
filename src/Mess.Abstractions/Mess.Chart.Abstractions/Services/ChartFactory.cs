using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Cms;
using OrchardCore.ContentManagement;
using System.Globalization;

namespace Mess.Chart.Abstractions.Services;

public abstract class ChartFactory<T> : IChartFactory
  where T : ContentItemBase
{
  protected abstract Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    T metadata,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  );

  public abstract IEnumerable<string> TimeseriesChartDatasetProperties { get; }

  public async Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  )
  {
    if (chart.ContentType == "TimeseriesChart")
    {
      var timeseriesChart = chart.AsContent<TimeseriesChartItem>();
      var timeseriesChartDatasets =
        timeseriesChart.TimeseriesChartPart.Value.Datasets.Select(
          dataset => dataset.AsContent<TimeseriesChartDatasetItem>()
        );

      return await CreateTimeseriesChartAsync(
        metadata.AsContent<T>(),
        timeseriesChart,
        timeseriesChartDatasets
      );
    }

    throw new InvalidOperationException(
      $"Invalid chart content type: {chart.ContentType}"
    );
  }

  protected bool ContainsTimeseriesProperty<TP>(string property) =>
    typeof(TP)
      .GetProperties()
      .Select(property => property.Name)
      .Contains(property);

  protected DateTimeOffset GetTimeseriesTimestamp(object data)
  {
    var propertyInfo =
      data.GetType().GetProperty("Timestamp")
      ?? throw new InvalidOperationException(
        "Timestamp property not found in timeseries data"
      );

    var value =
      propertyInfo.GetValue(data)
      ?? throw new InvalidOperationException("Invalid timeseries value");

    return (DateTimeOffset)value;
  }

  protected decimal GetTimeseriesValue(object data, string property)
  {
    var propertyInfo =
      data.GetType().GetProperty(property)
      ?? throw new InvalidOperationException(
        $"Invalid timeseries property: {property}"
      );

    var value = propertyInfo.GetValue(data);
    return value == null
      ? 0.0M
      : Convert.ToDecimal(value, CultureInfo.InvariantCulture);
  }

  public string ContentType => typeof(T).ContentTypeName();
}
