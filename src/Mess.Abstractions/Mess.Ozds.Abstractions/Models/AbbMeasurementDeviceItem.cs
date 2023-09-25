using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using Mess.MeasurementDevice.Abstractions.Models;
using OrchardCore.Title.Models;
using Mess.Chart.Abstractions.Models;

namespace Mess.Ozds.Abstractions.Models;

public class AbbMeasurementDeviceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<MeasurementDevicePart> MeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<AbbMeasurementDevicePart> AbbMeasurementDevicePart
  {
    get;
    private set;
  } = default!;

  public Lazy<ChartPart> ChartPart { get; private set; } = default!;

  private AbbMeasurementDeviceItem(ContentItem contentItem)
    : base(contentItem) { }
}