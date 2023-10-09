using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class PaymentIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<PaymentIndex>()
      .When(contentItem => contentItem.Has<InvoicePart>())
      .Map(contentItem =>
      {
        var invoicePart = contentItem.As<InvoicePart>();

        return new PaymentIndex
        {
          BillingContentItemId = invoicePart.BillingContentItemId,
          InvoiceContentItemId = contentItem.ContentItemId,
          ReceiptContentItemId = invoicePart.ReceiptContentItemId,
          LegalEntityContentItemId = invoicePart.LegalEntityContentItemId,
        };
      });
  }
}
