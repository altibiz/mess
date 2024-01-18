using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsCalculationPart : ContentPart
{
  public List<OzdsCalculationData> Calculations { get; set; } = default!;
}
