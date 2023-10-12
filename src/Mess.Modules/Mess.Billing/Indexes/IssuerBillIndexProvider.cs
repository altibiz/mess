using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class IssuerBillIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<IssuerBillIndex>()
      .When(
        contentItem =>
          contentItem.Has<InvoicePart>() || contentItem.Has<ReceiptPart>()
      )
      .Map(contentItem =>
      {
        var invoicePart = contentItem.As<InvoicePart>();
        if (invoicePart is not null)
        {
          return invoicePart.IssuerRepresentativeUserIds.Select(
            legalRepresentativeUserId =>
              new IssuerBillIndex
              {
                ContentItemId = contentItem.ContentItemId,
                BillingContentItemId = invoicePart.BillingContentItemId,
                InvoiceContentItemId = contentItem.ContentItemId,
                ReceiptContentItemId = invoicePart.ReceiptContentItemId,
                IssuerContentItemId = invoicePart.RecipientContentItemId,
                IssuerRepresentativeUserId = legalRepresentativeUserId,
              }
          );
        }

        var receiptPart = contentItem.As<ReceiptPart>();
        if (receiptPart is not null)
        {
          return receiptPart.IssuerRepresentativeUserIds.Select(
            legalRepresentativeUserId =>
              new IssuerBillIndex
              {
                ContentItemId = contentItem.ContentItemId,
                BillingContentItemId = receiptPart.BillingContentItemId,
                InvoiceContentItemId = receiptPart.InvoiceContentItemId,
                ReceiptContentItemId = contentItem.ContentItemId,
                IssuerContentItemId = receiptPart.RecipientContentItemId,
                IssuerRepresentativeUserId = legalRepresentativeUserId,
              }
          );
        }

        throw new NullReferenceException(
          $"Content item {contentItem.ContentItemId} has neither invoice nor receipt"
        );
      });
  }

  public IssuerBillIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
