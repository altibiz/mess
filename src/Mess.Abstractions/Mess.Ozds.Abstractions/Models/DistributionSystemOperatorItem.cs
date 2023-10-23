using Mess.Billing.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemOperatorItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<DistributionSystemOperatorPart> DistributionSystemOperatorPart
  {
    get;
    private set;
  } = default!;

  public Lazy<LegalEntityPart> LegalEntityPart { get; private set; } = default!;

  public Lazy<ListPart> ListPart { get; private set; } = default!;

  private DistributionSystemOperatorItem(ContentItem contentItem)
    : base(contentItem) { }
}
