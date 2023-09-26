using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.ViewModels;

public class OzdsMeasurementDeviceListViewModel
{
  public List<(
    ContentItem ContentItem,
    TitlePart TitlePart,
    OzdsMeasurementDevicePart OzdsMeasurementDevicePart,
    MeasurementDevicePart MeasurementDevicePart
  )> ContentItems { get; set; } = default!;
}
