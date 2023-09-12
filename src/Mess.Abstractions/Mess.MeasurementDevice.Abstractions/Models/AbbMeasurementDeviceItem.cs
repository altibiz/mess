using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class AbbMeasurementDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<AbbMeasurementDevicePart> AbbMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  private AbbMeasurementDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
