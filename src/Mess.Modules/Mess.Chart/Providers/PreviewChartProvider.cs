using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Mess.Fields.Abstractions;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Providers;

public class PreviewChartProvider : ChartProvider
{
  public const string ChartContentType = "Preview";

  public override string ContentType => ChartContentType;

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
      RefreshInterval: chart.TimeseriesChartPart.Value.RefreshInterval.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      History: chart.TimeseriesChartPart.Value.History.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      Datasets: datasetDescriptors.ToList().AsReadOnly()
    );
  }
}
