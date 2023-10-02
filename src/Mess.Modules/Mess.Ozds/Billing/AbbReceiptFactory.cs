using Mess.Billing.Abstractions.Receipts;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class AbbReceiptFactory : IReceiptFactory
{
  public string ContentType => throw new NotImplementedException();

  public Task<Receipt> Create(ContentItem contentItem, ContentItem invoice)
  {
    throw new NotImplementedException();
  }

  public Task<Receipt> CreateAsync(ContentItem contentItem, ContentItem invoice)
  {
    throw new NotImplementedException();
  }
}
