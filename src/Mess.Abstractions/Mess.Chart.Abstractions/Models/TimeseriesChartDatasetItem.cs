using Mess.Cms;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetItem : ContentItemBase
{
  private TimeseriesChartDatasetItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TimeseriesChartDatasetPart> TimeseriesChartDatasetPart
  {
    get;
    private set;
  } = default!;
}
