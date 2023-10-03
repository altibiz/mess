using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemUnitPart : ContentPart
{
  public ContentPickerField ClosedDistributionSystem { get; set; } = default!;
}
