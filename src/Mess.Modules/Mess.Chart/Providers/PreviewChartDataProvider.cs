using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Providers;

public class PreviewChartDataProvider : ChartProvider
{
  public const string ProviderId = "Preview";

  public override string ContentType => ProviderId;

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new string[] { };

  public override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    ContentItem metadata,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var now = DateTime.UtcNow;
    var datasetDescriptors = datasets.Select(
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
      RefreshInterval: chart
        .TimeseriesChartPart
        .Value
        .RefreshIntervalSpan
        .TotalMilliseconds,
      History: chart.TimeseriesChartPart.Value.HistorySpan.TotalMilliseconds,
      Datasets: datasetDescriptors.ToList().AsReadOnly()
    );
  }
}
