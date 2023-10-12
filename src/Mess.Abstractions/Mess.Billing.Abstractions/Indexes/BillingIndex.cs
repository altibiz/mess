using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class BillingIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string ContentType { get; set; } = default!;

  public string IssuerContentItemId { get; set; } = default!;

  public string RecipientContentItemId { get; set; } = default!;
}
