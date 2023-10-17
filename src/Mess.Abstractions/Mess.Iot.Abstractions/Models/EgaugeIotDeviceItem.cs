using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Iot.Abstractions.Models;

public class EgaugeIotDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> MeasurementDevicePart { get; private set; } =
    default!;

  public Lazy<EgaugeIotDevicePart> EgaugeMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  private EgaugeIotDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
