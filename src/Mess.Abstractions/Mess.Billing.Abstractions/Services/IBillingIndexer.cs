using OrchardCore.ContentManagement;
using Mess.Billing.Abstractions.Indexes;

namespace Mess.Billing.Abstractions.Services;

public interface IBillingIndexer
{
  public string ContentType { get; }

  public BillingIndex IndexBilling(ContentItem contentItem);

  public Task<BillingIndex> IndexBillingAsync(ContentItem contentItem);
}
