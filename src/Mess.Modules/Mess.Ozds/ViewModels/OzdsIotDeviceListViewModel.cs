using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.ViewModels;

public class OzdsIotDeviceListViewModel
{
  public List<(
    ContentItem ContentItem,
    TitlePart TitlePart,
    OzdsIotDevicePart OzdsIotDevicePart,
    IotDevicePart IotDevicePart
  )> ContentItems { get; set; } = default!;
}
