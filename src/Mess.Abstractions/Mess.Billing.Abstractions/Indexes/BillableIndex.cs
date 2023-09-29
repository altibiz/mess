using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class BillableIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;
}
