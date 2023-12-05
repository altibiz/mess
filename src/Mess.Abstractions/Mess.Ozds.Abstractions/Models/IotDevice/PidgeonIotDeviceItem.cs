using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class PidgeonIotDeviceItem : ContentItemBase
{
  private PidgeonIotDeviceItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> IotDevicePart { get; private set; } =
    default!;

  public Lazy<OzdsIotDevicePart> OzdsIotDevicePart { get; private set; } =
    default!;

  public Lazy<PidgeonIotDevicePart> PidgeonIotDevicePart { get; private set; } =
    default!;
}
