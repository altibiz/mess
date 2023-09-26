using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using Mess.Iot.Abstractions.Models;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class PidgeonMeasurementDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<OzdsMeasurementDevicePart> OzdsMeasurementDevicePart
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
