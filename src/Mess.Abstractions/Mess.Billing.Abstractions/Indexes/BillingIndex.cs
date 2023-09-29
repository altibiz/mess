using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class BillingIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;
}
