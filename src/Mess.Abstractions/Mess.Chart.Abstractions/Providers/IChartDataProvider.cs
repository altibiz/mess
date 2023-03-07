using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartDataProvider
{
  public string Id { get; }

  public Task<ChartModel?> CreateChartAsync(ContentItem contentItem);

  public bool IsValidTimeseriesChartDatasetProperty(string property);

  public IReadOnlyList<string> GetTimeseriesChartDatasetProperties();
}
