using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Chart.Indexes;

public class ChartIndex : MapIndex
{
  public string ChartContentItemId { get; set; } = default!;
  public string? ChartDataProviderId { get; set; } = default!;
  public string? Title { get; set; } = default!;
}

public class ChartIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<ChartIndex>()
      .When(
        contentItem =>
          contentItem.ContentType == TimeseriesChartItem.ContentType
      )
      .Map(contentItem =>
      {
        if (contentItem.ContentType == TimeseriesChartItem.ContentType)
        {
          var timeseriesChart = contentItem.AsContent<TimeseriesChartItem>();
          return new ChartIndex
          {
            ChartDataProviderId = timeseriesChart
              .TimeseriesChartPart
              .Value
              .ChartDataProviderId,
            ChartContentItemId = contentItem.ContentItemId,
            Title = timeseriesChart.TitlePart.Value.Title
          };
        }

        return new ChartIndex
        {
          ChartContentItemId = contentItem.ContentItemId
        };
      });
  }
}
