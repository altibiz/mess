using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartItem : ContentItemBase
{
  public const string ContentType = "TimeseriesChart";

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<TimeseriesChartPart> TimeseriesChartPart { get; private set; } =
    default!;

  private TimeseriesChartItem(ContentItem contentItem)
    : base(contentItem) { }
}
