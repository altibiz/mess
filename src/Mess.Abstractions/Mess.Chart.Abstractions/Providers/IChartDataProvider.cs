using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartDataProvider
{
  public string Id { get; }

  public ChartModel? CreateChart(ContentItem contentItem);

  public Task<ChartModel?> CreateChartAsync(ContentItem contentItem);
}
