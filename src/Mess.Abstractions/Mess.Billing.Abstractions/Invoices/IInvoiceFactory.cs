using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Invoices;

public interface IInvoiceFactory
{
  string ContentType { get; }

  Task<Invoice> Create(ContentItem contentItem);

  Task<Invoice> CreateAsync(ContentItem contentItem);
}
