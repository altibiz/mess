using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Providers;

public abstract class ChartDataProvider<TPart> : IChartDataProvider
  where TPart : ContentPart
{
  public abstract string Id { get; }

  public abstract ChartModel? CreateChart(TPart part);

  public abstract Task<ChartModel?> CreateChartAsync(TPart part);

  public virtual ChartModel? CreateChart(ContentItem contentItem)
  {
    var part = contentItem.As<TPart>();
    if (part is null)
    {
      return null;
    }

    return CreateChart(part);
  }

  public virtual async Task<ChartModel?> CreateChartAsync(
    ContentItem contentItem
  )
  {
    var part = contentItem.As<TPart>();
    if (part is null)
    {
      return null;
    }

    return await CreateChartAsync(part);
  }
}
