using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemUnitPart : ContentPart
{
  public decimal Consumption { get; set; } = default;
}
