using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class LegalEntityIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string ContentType { get; set; } = default!;
}
