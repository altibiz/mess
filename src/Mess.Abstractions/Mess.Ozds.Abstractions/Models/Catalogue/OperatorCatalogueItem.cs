using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class OperatorCatalogueItem : ContentItemBase
{
  private OperatorCatalogueItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = null!;

  public Lazy<OperatorCataloguePart> OperatorCataloguePart
  {
    get;
    private set;
  } = null!;
}
