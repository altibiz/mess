using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class RegulatoryAgencyCataloguePart : ContentPart
{
  public NumericField RenewableEnergyFeePrice { get; set; } = default!;

  public NumericField BusinessUsageFeePrice { get; set; } = default!;

  public NumericField TaxRate { get; set; } = default!;
}
