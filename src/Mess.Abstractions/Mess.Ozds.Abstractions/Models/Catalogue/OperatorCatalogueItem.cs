using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class OperatorCatalogueItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = null!;

  public Lazy<OperatorCataloguePart> OperatorCataloguePart
  {
    get;
    private set;
  } = null!;

  private OperatorCatalogueItem(ContentItem contentItem)
    : base(contentItem) { }
}
