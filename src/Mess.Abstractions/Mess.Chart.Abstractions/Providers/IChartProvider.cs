using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartProvider
{
  public string Id { get; }

  public Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  );

  public IEnumerable<string> TimeseriesChartDatasetProperties { get; }
}
