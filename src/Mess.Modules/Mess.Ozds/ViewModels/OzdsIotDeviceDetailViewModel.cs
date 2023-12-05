using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.ViewModels;

public class OzdsIotDeviceDetailViewModel
{
  public ContentItem ContentItem { get; set; } = default!;

  public OzdsIotDevicePart OzdsIotDevicePart { get; set; } = default!;

  public IotDevicePart IotDevicePart { get; set; } = default!;
}
