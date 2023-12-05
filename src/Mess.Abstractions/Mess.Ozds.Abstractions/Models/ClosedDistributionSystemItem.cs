using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;
using Mess.Billing.Abstractions.Models;
using OrchardCore.Lists.Models;

namespace Mess.Ozds.Abstractions.Models;

public class ClosedDistributionSystemItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<ClosedDistributionSystemPart> ClosedDistributionSystemPart
  {
    get;
    private set;
  } = default!;

  public Lazy<LegalEntityPart> LegalEntityPart { get; private set; } = default!;

  public Lazy<ListPart> ListPart { get; private set; } = default!;

  private ClosedDistributionSystemItem(ContentItem contentItem)
    : base(contentItem) { }
}
