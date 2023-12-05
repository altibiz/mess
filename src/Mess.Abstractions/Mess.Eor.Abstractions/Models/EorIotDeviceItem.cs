using Mess.Cms;
using OrchardCore.ContentManagement;
using Mess.Iot.Abstractions.Models;
using OrchardCore.Title.Models;
using Mess.Chart.Abstractions.Models;

namespace Mess.Eor.Abstractions.Models;

public class EorIotDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> IotDevicePart { get; private set; } = default!;

  public Lazy<EorIotDevicePart> EorIotDevicePart { get; private set; } =
    default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  private EorIotDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
