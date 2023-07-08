using Mess.MeasurementDevice.Abstractions.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class MeasurementDevicePart : ContentPart
{
  public TextField DeviceId { get; set; } = default!;

  public ApiKeyField ApiKey { get; set; } = default!;

  public string PushHandlerId { get; set; } = default!;

  public string UpdateHandlerId { get; set; } = default!;

  public string PollHandlerId { get; set; } = default!;
}
