using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Models;

public class IotDevicePart : ContentPart
{
  public TextField DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; }
}
