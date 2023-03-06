using Mess.OrchardCore;
using OrchardCore.Title.Models;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.Chart.Abstractions.Models;

public class ChartItem : ContentItem
{
  public Lazy<TitlePart> TitlePart { get; set; } = default!;

  public Lazy<ChartPart> ChartPart { get; set; } = default!;

  private ChartItem(OrchardContentItem inner) : base(inner) { }
}
