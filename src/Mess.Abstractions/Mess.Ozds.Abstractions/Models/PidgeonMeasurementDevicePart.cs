using Mess.ContentFields.Abstractions.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class PidgeonMeasurementDevicePart : ContentPart
{
  public ApiKeyField ApiKey { get; set; } = default!;
}
