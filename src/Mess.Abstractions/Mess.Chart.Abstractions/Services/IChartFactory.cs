using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Services;

public interface IChartFactory
{
  public string ContentType { get; }

  public IEnumerable<string> TimeseriesChartDatasetProperties { get; }

  public Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  );
}
