using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class RegulatoryAgencyCatalogueItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = null!;

  public Lazy<RegulatoryAgencyCataloguePart> RegulatoryAgencyCataloguePart
  {
    get;
    private set;
  } = null!;

  private RegulatoryAgencyCatalogueItem(ContentItem contentItem)
    : base(contentItem) { }
}
