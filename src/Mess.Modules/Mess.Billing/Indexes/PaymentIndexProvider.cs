using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql;
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

        return invoicePart.Invoice.CatalogueContentItemIds.Select(
          catalogueContentItemId =>
            new PaymentIndex
            {
              InvoiceContentItemId = contentItem.ContentItemId,
              ReceiptContentItemId = invoicePart.Invoice.ReceiptContentItemId,
              IssuerContentItemId = invoicePart.Invoice.IssuerContentItemId,
              RecipientContentItemId = invoicePart
                .Invoice
                .RecipientContentItemId,
              BillingContentItemId = invoicePart.Invoice.BillableContnetItemId,
              CatalogueContentItemId = catalogueContentItemId
            }
        );
      });
  }
}
