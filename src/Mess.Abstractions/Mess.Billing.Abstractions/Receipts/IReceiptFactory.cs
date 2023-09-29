using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Receipts;

public interface IReceiptFactory
{
  string ContentType { get; }

  Task<Receipt> Create(ContentItem contentItem, ContentItem invoice);

  Task<Receipt> CreateAsync(ContentItem contentItem, ContentItem invoice);
}
