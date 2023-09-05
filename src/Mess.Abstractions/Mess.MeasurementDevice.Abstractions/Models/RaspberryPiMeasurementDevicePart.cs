using Mess.ContentFields.Abstractions.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class RaspberryPiMeasurementDevicePart : ContentPart
{
  public ApiKeyField ApiKey { get; set; } = default!;
}
