using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.ViewModels;

public class OzdsIotDeviceListViewModel
{
  public List<(
    ContentItem ContentItem,
    OzdsIotDevicePart OzdsIotDevicePart,
    IotDevicePart IotDevicePart
  )> ContentItems
  { get; set; } = default!;
}
