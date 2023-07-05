using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class EgaugeMeasurementDeviceItem : ContentItemBase
{
  public const string ContentType = "EgaugeMeasurementDevice";

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<EgaugeMeasurementDevicePart> EgaugeMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  private EgaugeMeasurementDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
