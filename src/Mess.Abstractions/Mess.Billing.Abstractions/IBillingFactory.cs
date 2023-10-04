using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions;

public interface IBillingFactory
{
  string ContentType { get; }

  ContentItem CreateInvoice(ContentItem contentItem, ContentItem[] catalogueContentItems);

  Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    ContentItem[] catalogueContentItems
  );

  ContentItem CreateReceipt(ContentItem contentItem, ContentItem invoiceContentItem);

  Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );
}
