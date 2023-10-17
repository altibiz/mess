using Mess.Billing.Abstractions.Indexes;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public abstract class BillingIndexer<T> : IBillingIndexer
  where T : ContentItemBase
{
  protected abstract BillingIndex IndexBilling(T contentItem);

  protected abstract Task<BillingIndex> IndexBillingAsync(T contentItem);

  public string ContentType
  {
    get => typeof(T).ContentTypeName();
  }

  public BillingIndex IndexBilling(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    if (item is null)
    {
      throw new NullReferenceException(
        $"Content item {contentItem.ContentItemId} is not of type {typeof(T).Name}"
      );
    }

    return IndexBilling(item);
  }

  public async Task<BillingIndex> IndexBillingAsync(ContentItem contentItem)
  {
    var item = contentItem.AsContent<T>();
    if (item is null)
    {
      throw new NullReferenceException(
        $"Content item {contentItem.ContentItemId} is not of type {typeof(T).Name}"
      );
    }

    return await IndexBillingAsync(item);
  }
}
