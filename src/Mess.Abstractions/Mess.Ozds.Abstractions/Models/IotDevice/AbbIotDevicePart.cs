using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class AbbIotDevicePart : ContentPart
{
  public NumericField MinVoltage_V { get; set; } = default!;

  public NumericField MaxVoltage_V { get; set; } = default!;

  public NumericField MinCurrent_A { get; set; } = default!;

  public NumericField MaxCurrent_A { get; set; } = default!;

  public NumericField MinActivePower_W { get; set; } = default!;

  public NumericField MaxActivePower_W { get; set; } = default!;

  public NumericField MinReactivePower_VAR { get; set; } = default!;

  public NumericField MaxReactivePower_VAR { get; set; } = default!;
}
