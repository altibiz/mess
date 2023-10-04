using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class RegulatoryAgencyCataloguePart : ContentPart
{
  public NumericField RenewableEnergyFee { get; set; } = default!;

  public NumericField BusinessUsageFee { get; set; } = default!;

  public NumericField TaxRate { get; set; } = default!;
}
