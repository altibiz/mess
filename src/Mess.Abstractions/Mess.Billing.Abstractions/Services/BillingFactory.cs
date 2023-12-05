using Mess.Cms;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public abstract class BillingFactory<T> : IBillingFactory
  where T : ContentItemBase
{
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
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return CreateInvoice(contentItem.AsContent<T>(), fromDate, toDate);
  }

  public async Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await CreateInvoiceAsync(
      contentItem.AsContent<T>(),
      fromDate,
      toDate
    );
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == typeof(T).ContentTypeName();
  }

  public abstract ContentItem CreateInvoice(
    T contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public abstract Task<ContentItem> CreateInvoiceAsync(
    T contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
