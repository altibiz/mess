using Mess.OrchardCore;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetItem : ContentItem
{
  public Lazy<TimeseriesChartDatasetPart> TimeseriesChartDatasetPart { get; set; } =
    default!;

  private TimeseriesChartDatasetItem(OrchardContentItem inner) : base(inner) { }
}
