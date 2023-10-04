using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions;

public interface IBillingFactory
{
  string ContentType { get; }

  ContentItem CreateInvoice(ContentItem contentItem);

  Task<ContentItem> CreateInvoiceAsync(ContentItem contentItem);

  ContentItem CreateReceipt(ContentItem contentItem, ContentItem invoiceContentItem);

  Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );
}
