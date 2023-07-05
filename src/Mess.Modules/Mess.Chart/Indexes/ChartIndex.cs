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
      .When(contentItem => contentItem.ContentType.EndsWith("Chart"))
      .Map(
        contentItem =>
          new ChartIndex
          {
            ChartContentItemId = contentItem.ContentItemId,
            ChartDataProviderId = contentItem.Content[
              $"{contentItem.ContentType}Part"
            ].ChartDataProviderId,
            Title = contentItem.Content.TitlePart.Title
          }
      );
  }
}
