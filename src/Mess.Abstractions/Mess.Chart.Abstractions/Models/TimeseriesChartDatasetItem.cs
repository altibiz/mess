using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetItem : ContentItemBase
{
  public const string ContentType = "TimeseriesChartDataset";

  public Lazy<TimeseriesChartDatasetPart> TimeseriesChartDatasetPart
  {
    get;
    private set;
  } = default!;

  private TimeseriesChartDatasetItem(ContentItem contentItem)
    : base(contentItem) { }
}
