using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class BillingIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<BillingIndex>()
      .When(contentItem => contentItem.Has<BillingPart>())
      .Map(contentItem =>
      {
        var billablePart = contentItem.As<BillingPart>();

        return new BillingIndex { ContentItemId = contentItem.ContentItemId, };
      });
  }
}
