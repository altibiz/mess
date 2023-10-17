using Mess.Billing.Abstractions.Indexes;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public abstract class PaymentIndexer<T> : IPaymentIndexer
  where T : ContentItemBase
{
  protected abstract PaymentIndex IndexPayment(T contentItem);

  protected abstract Task<PaymentIndex> IndexPaymentAsync(T contentItem);

  public PaymentIndex IndexPayment(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    if (item is null)
    {
      throw new NullReferenceException(
        $"Content item {contentItem.ContentItemId} is not of type {typeof(T).Name}"
      );
    }

    return IndexPayment(item);
  }

  public async Task<PaymentIndex> IndexPaymentAsync(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    if (item is null)
    {
      throw new NullReferenceException(
        $"Content item {contentItem.ContentItemId} is not of type {typeof(T).Name}"
      );
    }

    return await IndexPaymentAsync(item);
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == typeof(T).ContentTypeName();
  }
}
