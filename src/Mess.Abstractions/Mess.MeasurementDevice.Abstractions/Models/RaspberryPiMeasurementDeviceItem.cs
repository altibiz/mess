using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class RaspberryPiMeasurementDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<RaspberryPiMeasurementDevicePart> RaspberryPiMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  private RaspberryPiMeasurementDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
