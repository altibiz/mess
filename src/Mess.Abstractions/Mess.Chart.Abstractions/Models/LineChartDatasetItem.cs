using Mess.OrchardCore;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.Chart.Abstractions.Models;

public class LineChartDatasetItem : ContentItem
{
  public Lazy<LineChartDatasetPart> LineChartDatasetPart { get; set; } =
    default!;

  private LineChartDatasetItem(OrchardContentItem inner) : base(inner) { }
}
