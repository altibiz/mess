using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Receipts;

public interface IReceiptFactory
{
  string ContentType { get; }

  Receipt Create(ContentItem contentItem, ContentItem invoiceContentItem);

  Task<Receipt> CreateAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );
}
