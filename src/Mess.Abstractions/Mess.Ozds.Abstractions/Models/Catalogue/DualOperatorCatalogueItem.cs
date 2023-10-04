using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class DualOperatorCatalogueItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = null!;

  public Lazy<DualOperatorCataloguePart> DualOperatorCataloguePart
  {
    get;
    private set;
  } = null!;

  private DualOperatorCatalogueItem(ContentItem contentItem)
    : base(contentItem) { }
}
