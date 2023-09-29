using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class BillableIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<BillableIndex>()
      .When(contentItem => contentItem.Has<BillablePart>())
      .Map(contentItem =>
      {
        var billablePart = contentItem.As<BillablePart>();

        return new BillableIndex { ContentItemId = contentItem.ContentItemId, };
      });
  }
}
