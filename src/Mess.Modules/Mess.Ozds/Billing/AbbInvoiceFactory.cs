using Mess.Billing.Abstractions.Invoices;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class AbbInvoiceFactory : IInvoiceFactory
{
  public string ContentType => throw new NotImplementedException();

  public Task<Invoice> Create(ContentItem contentItem)
  {
    throw new NotImplementedException();
  }

  public Task<Invoice> CreateAsync(ContentItem contentItem)
  {
    throw new NotImplementedException();
  }
}
