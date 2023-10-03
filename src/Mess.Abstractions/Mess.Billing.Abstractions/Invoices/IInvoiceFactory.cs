using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Invoices;

public interface IInvoiceFactory
{
  string ContentType { get; }

  Invoice Create(ContentItem contentItem, ContentItem[] catalogueContentItems);

  Task<Invoice> CreateAsync(
    ContentItem contentItem,
    ContentItem[] catalogueContentItems
  );
}
