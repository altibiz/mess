using Mess.Billing.Abstractions.Indexes;
using Mess.Cms;
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
    return IndexPayment(item);
  }

  public async Task<PaymentIndex> IndexPaymentAsync(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    return await IndexPaymentAsync(item);
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == typeof(T).ContentTypeName();
  }
}
