using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class SchneiderIotDevicePart : ContentPart
{
  public decimal? LatestImport { get; set; } = default!;
}
