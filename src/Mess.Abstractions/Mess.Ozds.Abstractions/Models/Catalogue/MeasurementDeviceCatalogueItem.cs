using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class MeasurementDeviceCatalogueItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = null!;

  public Lazy<MeasurementDeviceCataloguePart> MeasurementDeviceCataloguePart { get; private set; } = null!;

  private MeasurementDeviceCatalogueItem(ContentItem contentItem)
    : base(contentItem) { }
}
