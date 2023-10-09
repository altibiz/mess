using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Mess.Fields.Abstractions;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;

namespace Mess.Chart.Providers;

public class PreviewChartProvider : IChartProvider
{
  public const string ChartContentType = "Preview";

  public string ContentType => ChartContentType;

  public IEnumerable<string> TimeseriesChartDatasetProperties =>
    new string[] { };

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
                    Y: Random.Shared.NextSingle() * 100
                  )
              )
              .Reverse()
              .ToList()
              .AsReadOnly()
          )
      );

      return new TimeseriesChartDescriptor(
        RefreshInterval: timeseriesChart.TimeseriesChartPart.Value.RefreshInterval.Value
          .ToTimeSpan()
          .TotalMilliseconds,
        History: timeseriesChart.TimeseriesChartPart.Value.History.Value
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
