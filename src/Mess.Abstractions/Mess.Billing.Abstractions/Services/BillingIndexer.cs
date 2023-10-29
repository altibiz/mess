using Mess.Billing.Abstractions.Indexes;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public abstract class BillingIndexer<T> : IBillingIndexer
  where T : ContentItemBase
{
  protected abstract BillingIndex IndexBilling(T contentItem);

  protected abstract Task<BillingIndex> IndexBillingAsync(T contentItem);

  public BillingIndex IndexBilling(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    return IndexBilling(item);
  }

  public async Task<BillingIndex> IndexBillingAsync(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    return await IndexBillingAsync(item);
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == typeof(T).ContentTypeName();
  }
}
