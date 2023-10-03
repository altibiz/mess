using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class LegalEntityIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<LegalEntityIndex>()
      .When(contentItem => contentItem.Has<LegalEntityPart>())
      .Map(contentItem =>
      {
        var legalEntityPart = contentItem.As<LegalEntityPart>();

        return new LegalEntityIndex
        {
          ContentItemId = contentItem.ContentItemId,
          ContentType = contentItem.ContentType
        };
      });
  }
}
