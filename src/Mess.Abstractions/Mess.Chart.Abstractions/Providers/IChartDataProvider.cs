using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartDataProvider
{
  public string Id { get; }

  public Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  );

  public IEnumerable<string> TimeseriesChartDatasetProperties { get; }
}
