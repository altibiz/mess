using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartProvider
{
  public string ContentType { get; }

  public Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  );

  public IEnumerable<string> TimeseriesChartDatasetProperties { get; }
}
