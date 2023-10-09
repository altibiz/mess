using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public abstract class ChartProvider<T> : IChartProvider
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

  protected bool ContainsTimeseriesProperty<P>(string property) =>
    typeof(P)
      .GetProperties()
      .Select(property => property.Name)
      .Contains(property);

  protected DateTimeOffset GetTimeseriesTimestamp(object data)
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

    return (DateTimeOffset)value;
  }

  protected float GetTimeseriesValue(object data, string property)
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

    return Convert.ToSingle(value);
  }

  public string ContentType => typeof(T).ContentTypeName();
}
