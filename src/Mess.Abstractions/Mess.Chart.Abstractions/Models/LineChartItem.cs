using Mess.OrchardCore;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.Chart.Abstractions.Models;

public class LineChartItem : ContentItem
{
  public Lazy<LineChartPart> LineChartPart { get; set; } = default!;

  private LineChartItem(OrchardContentItem inner) : base(inner) { }
}
