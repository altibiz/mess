using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Factory;

public abstract class BillingFactory<T> : IBillingFactory
  where T : ContentItemBase
{
  public string ContentType => typeof(T).ContentTypeName();

  public abstract ContentItem CreateInvoice(
    T contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  );

  public abstract Task<ContentItem> CreateInvoiceAsync(
    T contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  );

  public abstract ContentItem CreateReceipt(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );

  public abstract Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  );

  public ContentItem CreateInvoice(
    ContentItem contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    return CreateInvoice(contentItem.AsContent<T>(), from, to);
  }

  public async Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    DateTimeOffset from,
    DateTimeOffset to
  )
  {
    return await CreateInvoiceAsync(contentItem.AsContent<T>(), from, to);
  }
}
