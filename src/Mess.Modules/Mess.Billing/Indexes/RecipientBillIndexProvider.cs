using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class RecipientBillIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<RecipientBillIndex>()
      .When(
        contentItem =>
          contentItem.Has<InvoicePart>() || contentItem.Has<ReceiptPart>()
      )
      .Map(contentItem =>
      {
        var invoicePart = contentItem.As<InvoicePart>();
        if (invoicePart is not null)
        {
          return invoicePart.RecipientRepresentativeUserIds.Select(
            legalRepresentativeUserId =>
              new RecipientBillIndex
              {
                ContentItemId = contentItem.ContentItemId,
                BillingContentItemId = invoicePart.BillingContentItemId,
                InvoiceContentItemId = contentItem.ContentItemId,
                ReceiptContentItemId = invoicePart.ReceiptContentItemId,
                RecipientContentItemId = invoicePart.RecipientContentItemId,
                RecipientRepresentativeUserId = legalRepresentativeUserId,
              }
          );
        }

        var receiptPart = contentItem.As<ReceiptPart>();
        if (receiptPart is not null)
        {
          return receiptPart.RecipientRepresentativeUserIds.Select(
            legalRepresentativeUserId =>
              new RecipientBillIndex
              {
                ContentItemId = contentItem.ContentItemId,
                BillingContentItemId = receiptPart.BillingContentItemId,
                InvoiceContentItemId = receiptPart.InvoiceContentItemId,
                ReceiptContentItemId = contentItem.ContentItemId,
                RecipientContentItemId = receiptPart.RecipientContentItemId,
                RecipientRepresentativeUserId = legalRepresentativeUserId,
              }
          );
        }

        throw new NullReferenceException(
          $"Content item {contentItem.ContentItemId} has neither invoice nor receipt"
        );
      });
  }

  public RecipientBillIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
