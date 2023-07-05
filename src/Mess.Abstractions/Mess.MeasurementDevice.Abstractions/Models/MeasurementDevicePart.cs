using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class MeasurementDevicePart : ContentPart
{
  public TextField DeviceId { get; set; } = default!;

  public string? DefaultPushHandlerId { get; set; } = default!;

  public string? DefaultPollHandlerId { get; set; } = default!;

  public string? DefaultUpdateHandlerId { get; set; } = default!;
}
