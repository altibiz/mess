using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class CatalogueIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string ContentType { get; set; } = default!;
}
