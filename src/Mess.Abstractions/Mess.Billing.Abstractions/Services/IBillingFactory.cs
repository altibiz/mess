using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public interface IBillingFactory
{
  public bool IsApplicable(ContentItem contentItem);

  ContentItem CreateInvoice(
    ContentItem contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  );

  Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  );

  ContentItem CreateReceipt(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );

  Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );
}
