using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class CatalogueIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<CatalogueIndex>()
      .When(contentItem => contentItem.Has<CataloguePart>())
      .Map(contentItem =>
      {
        var cataloguePart = contentItem.As<CataloguePart>();

        return new CatalogueIndex
        {
          ContentItemId = contentItem.ContentItemId,
          ContentType = contentItem.ContentType
        };
      });
  }
}
