using Mess.Chart.Abstractions.Models;
using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Enms.Abstractions.Models;

public class EgaugeIotDeviceItem : ContentItemBase
{
  private EgaugeIotDeviceItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> IotDevicePart { get; private set; } = default!;

  public Lazy<EgaugeIotDevicePart> EgaugeIotDevicePart { get; private set; } =
    default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;
}
