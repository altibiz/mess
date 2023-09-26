using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using Mess.Iot.Abstractions.Models;
using OrchardCore.Title.Models;
using Mess.Chart.Abstractions.Models;

namespace Mess.Eor.Abstractions.Models;

public class EorMeasurementDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<EorMeasurementDevicePart> EorMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  private EorMeasurementDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}
