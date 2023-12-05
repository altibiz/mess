using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.Fields.Abstractions;
using OrchardCore.ContentManagement;
using Mess.Cms;

namespace Mess.Chart.Providers;

public class PreviewChartFactory : IChartFactory
{
  public const string ChartContentType = "Preview";

  public string ContentType => ChartContentType;

  public IEnumerable<string> TimeseriesChartDatasetProperties =>
    Array.Empty<string>();

  public async Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  )
  {
    var now = DateTimeOffset.UtcNow;
    if (chart.ContentType == "TimeseriesChart")
    {
      var timeseriesChart = chart.AsContent<TimeseriesChartItem>();
      var timeseriesChartDatasets =
        timeseriesChart.TimeseriesChartPart.Value.Datasets.Select(
          dataset => dataset.AsContent<TimeseriesChartDatasetItem>()
        );

      var datasetDescriptors = timeseriesChartDatasets.Select(
        dataset =>
          new TimeseriesChartDatasetDescriptor(
            Color: dataset.TimeseriesChartDatasetPart.Value.Color.Value,
            Label: dataset.TimeseriesChartDatasetPart.Value.Label.Text,
            Datapoints: Enumerable
              .Range(0, 100)
              .Select(
                i =>
                  new TimeseriesChartDatapointDescriptor(
                    X: now.Subtract(TimeSpan.FromMinutes(i)),
                    Y: (decimal)Random.Shared.NextDouble() * 100
                  )
              )
              .Reverse()
              .ToList()
              .AsReadOnly()
          )
      );

      return new TimeseriesChartDescriptor(
        RefreshInterval: (decimal)
          timeseriesChart.TimeseriesChartPart.Value.RefreshInterval.Value
            .ToTimeSpan()
            .TotalMilliseconds,
        History: (decimal)
          timeseriesChart.TimeseriesChartPart.Value.History.Value
            .ToTimeSpan()
            .TotalMilliseconds,
        Datasets: datasetDescriptors.ToList().AsReadOnly()
      );
    }

    throw new InvalidOperationException(
      $"Invalid chart content type: {chart.ContentType}"
    );
  }
}
