using Mess.OrchardCore;
using OrchardCore.ContentManagement;
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

  private DistributionSystemOperatorItem(ContentItem contentItem)
    : base(contentItem) { }
}
