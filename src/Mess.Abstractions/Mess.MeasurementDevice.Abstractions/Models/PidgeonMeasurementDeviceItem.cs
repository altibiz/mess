using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class PidgeonMeasurementDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<PidgeonMeasurementDevicePart> PidgeonMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  private PidgeonMeasurementDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
