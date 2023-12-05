using Mess.Billing.Abstractions.Indexes;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public interface IBillingIndexer
{
  public bool IsApplicable(ContentItem contentItem);

  public BillingIndex IndexBilling(ContentItem contentItem);

  public Task<BillingIndex> IndexBillingAsync(ContentItem contentItem);
}
