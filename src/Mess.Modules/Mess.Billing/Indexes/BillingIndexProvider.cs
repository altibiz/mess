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
        var billingPart = contentItem.As<BillingPart>();

        return billingPart.CatalogueContentItemIds.Select(
          catalogueContentItemId =>
            new BillingIndex
            {
              ContentItemId = contentItem.ContentItemId,
              ContentType = contentItem.ContentType,
              LegalEntityContentItemId = billingPart.LegalEntityContentItemId,
              CatalogueContentItemId = catalogueContentItemId,
            }
        );
      });
  }
}
