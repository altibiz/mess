using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class AbbIotDevicePart : ContentPart
{
  public decimal? LatestImport { get; set; } = default!;
}
