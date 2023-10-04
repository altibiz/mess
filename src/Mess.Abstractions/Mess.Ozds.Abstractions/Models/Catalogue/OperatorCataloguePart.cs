using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

// TODO: validation
public class OperatorCataloguePart : ContentPart
{
  public TextField Voltage { get; set; } = default!;

  public TextField Model { get; set; } = default!;

  public NumericField HighEnergyPrice { get; set; } = new() { Value = 0.00M };

  public NumericField LowEnergyPrice { get; set; } = new() { Value = 0.00M };

  public NumericField EnergyPrice { get; set; } = new() { Value = 0.00M };

  public NumericField MaxPowerPrice { get; set; } = new() { Value = 0.00M };

  public NumericField MeasurementDeviceFee { get; set; } = new() { Value = 0.00M };
}
