using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class AbbIotDevicePart : ContentPart
{
  public NumericField MinVoltage { get; set; } = default!;

  public NumericField MaxVoltage { get; set; } = default!;

  public NumericField MinCurrent { get; set; } = default!;

  public NumericField MaxCurrent { get; set; } = default!;

  public NumericField MinActivePower { get; set; } = default!;

  public NumericField MaxActivePower { get; set; } = default!;

  public NumericField MinReactivePower { get; set; } = default!;

  public NumericField MaxReactivePower { get; set; } = default!;
}
