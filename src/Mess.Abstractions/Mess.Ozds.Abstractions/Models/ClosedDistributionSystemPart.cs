using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class ClosedDistributionSystemPart : ContentPart
{
  public decimal Consumption { get; set; } = default!;
}
