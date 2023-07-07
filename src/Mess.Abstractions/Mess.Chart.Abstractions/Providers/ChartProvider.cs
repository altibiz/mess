using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public abstract class ChartProvider : IChartProvider
{
  public abstract string Id { get; }

  public abstract IEnumerable<string> TimeseriesChartDatasetProperties { get; }

  public virtual async Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  )
  {
    if (chart.ContentType == TimeseriesChartItem.ContentType)
    {
      var timeseriesChart = chart.AsContent<TimeseriesChartItem>();
      var timeseriesChartDatasets =
        timeseriesChart.TimeseriesChartPart.Value.Datasets.Select(
          dataset => dataset.AsContent<TimeseriesChartDatasetItem>()
        );

      return await CreateTimeseriesChartAsync(
        metadata,
        timeseriesChart,
        timeseriesChartDatasets
      );
    }

    throw new InvalidOperationException(
      $"Invalid chart content type: {chart.ContentType}"
    );
  }

  public virtual bool ContainsTimeseriesProperty<T>(string property) =>
    typeof(T)
      .GetProperties()
      .Select(property => property.Name)
      .Contains(property);

  public virtual DateTime GetTimeseriesTimestamp(object data)
  {
    var propertyInfo = data.GetType().GetProperty("Timestamp");
    if (propertyInfo == null)
    {
      throw new InvalidOperationException(
        $"Invalid timeseries property: {propertyInfo}"
      );
    }

    var value = propertyInfo.GetValue(data);
    if (value == null)
    {
      throw new InvalidOperationException($"Invalid timeseries value: {value}");
    }

    return (DateTime)value;
  }

  public virtual float GetTimeseriesValue(object data, string property)
  {
    var propertyInfo = data.GetType().GetProperty(property);
    if (propertyInfo == null)
    {
      throw new InvalidOperationException(
        $"Invalid timeseries property: {propertyInfo}"
      );
    }

    var value = propertyInfo.GetValue(data);
    if (value == null)
    {
      throw new InvalidOperationException($"Invalid timeseries value: {value}");
    }

    return (float)value;
  }

  public abstract Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    ContentItem metadata,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  );
}
