using Mess.Billing.Abstractions.Models;
using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemUnitItem : ContentItemBase
{
  private DistributionSystemUnitItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<DistributionSystemUnitPart> DistributionSystemUnitPart
  {
    get;
    private set;
  } = default!;

  public Lazy<LegalEntityPart> LegalEntityPart { get; private set; } = default!;

  public Lazy<ListPart> ListPart { get; private set; } = default!;
}
