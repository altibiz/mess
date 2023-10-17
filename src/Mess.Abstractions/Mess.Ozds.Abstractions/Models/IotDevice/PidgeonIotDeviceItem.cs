using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using Mess.Iot.Abstractions.Models;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class PidgeonIotDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> IotDevicePart { get; private set; } =
    default!;

  public Lazy<OzdsIotDevicePart> OzdsIotDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<PidgeonIotDevicePart> PidgeonIotDevicePart
  {
    get;
    private set;
  } = default!;

  private PidgeonIotDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
