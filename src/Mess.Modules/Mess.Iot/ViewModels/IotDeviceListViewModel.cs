using OrchardCore.ContentManagement;

namespace Mess.Iot.ViewModels;

public class IotDeviceListViewModel
{
  public IList<ContentItem> Devices { get; set; } = default!;
}
