using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class DualOperatorCataloguePart : ContentPart
{
  public TextField Model { get; set; } = default!;

  public NumericField HighEnergyPrice { get; set; } = default!;

  public NumericField LowEnergyPrice { get; set; } = default!;

  public NumericField MaxPowerPrice { get; set; } = default!;

  public NumericField MeasurementDeviceFee { get; set; } = default!;
}
