using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class RegulatoryAgencyCataloguePart : ContentPart
{
  public NumericField HighEnergyPrice { get; set; } = new() { Value = 0.00M };

  public NumericField LowEnergyPrice { get; set; } = new() { Value = 0.00M };

  public NumericField RenewableEnergyFee { get; set; } = new() { Value = 0.00M };

  public NumericField BusinessUsageFee { get; set; } = new() { Value = 0.00M };

  public NumericField TaxRate { get; set; } = new() { Value = 0.00M };
}
