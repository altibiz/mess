using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class EgaugeMeasurementDevicePart : ContentPart
{
  public TextField DeviceId { get; set; } = default!;
}
