using Mess.Billing.Abstractions.Models;
using Mess.Chart.Abstractions.Models;
using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class AbbIotDeviceItem : ContentItemBase
{
  private AbbIotDeviceItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<IotDevicePart> IotDevicePart { get; private set; } =
    default!;

  public Lazy<OzdsIotDevicePart> OzdsIotDevicePart { get; private set; } =
    default!;

  public Lazy<AbbIotDevicePart> AbbIotDevicePart { get; private set; } =
    default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  public Lazy<BillingPart> BillingPart { get; private set; } = default!;
}
