using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Models;

public class MeasurementDevicePart : ContentPart
{
  public TextField DeviceId { get; set; } = default!;
}
