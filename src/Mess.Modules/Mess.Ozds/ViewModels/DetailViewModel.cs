using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.ViewModels;

public class OzdsMeasurementDeviceDetailViewModel
{
  public ContentItem ContentItem { get; set; } = default!;
  public TitlePart TitlePart { get; set; } = default!;
  public OzdsMeasurementDevicePart OzdsMeasurementDevicePart { get; set; } =
    default!;
  public IotDevicePart MeasurementDevicePart { get; set; } = default!;
}
