using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OperatorCataloguePart : ContentPart
{
  public TextField Model { get; set; } = default!;

  public NumericField EnergyPrice { get; set; } = default!;

  public NumericField MaxPowerPrice { get; set; } = default!;

  public NumericField MeasurementDeviceFee { get; set; } = default!;
}
