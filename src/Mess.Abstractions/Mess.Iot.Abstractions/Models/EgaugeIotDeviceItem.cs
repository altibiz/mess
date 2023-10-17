using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Iot.Abstractions.Models;

public class EgaugeIotDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> IotDevicePart { get; private set; } =
    default!;

  public Lazy<EgaugeIotDevicePart> EgaugeIotDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  private EgaugeIotDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
