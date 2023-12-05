using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;
using Mess.Billing.Abstractions.Models;
using OrchardCore.Lists.Models;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemUnitItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<DistributionSystemUnitPart> DistributionSystemUnitPart
  {
    get;
    private set;
  } = default!;

  public Lazy<LegalEntityPart> LegalEntityPart { get; private set; } = default!;

  public Lazy<ListPart> ListPart { get; private set; } = default!;

  private DistributionSystemUnitItem(ContentItem contentItem)
    : base(contentItem) { }
}
