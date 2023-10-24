using Mess.Fields.Abstractions.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class PidgeonIotDevicePart : ContentPart
{
  public ApiKeyField ApiKey { get; set; } = default!;
}
