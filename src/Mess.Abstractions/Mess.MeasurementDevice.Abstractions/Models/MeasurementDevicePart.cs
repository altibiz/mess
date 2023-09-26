using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class MeasurementDevicePart : ContentPart
{
  public TextField DeviceId { get; set; } = default!;

  public bool IsMessenger { get; set; } = false;
}
