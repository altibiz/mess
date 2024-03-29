using Mess.Fields.Abstractions.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Eor.Abstractions.Models;

public class EorIotDevicePart : ContentPart
{
  public UserPickerField Owner { get; set; } = default!;

  public DateField ManufactureDate { get; set; } = default!;

  public TextField Manufacturer { get; set; } = default!;

  public DateField CommisionDate { get; set; } = default!;

  public TextField ProductNumber { get; set; } = default!;

  public NumericField Latitude { get; set; } = default!;

  public NumericField Longitude { get; set; } = default!;

  public ApiKeyField ApiKey { get; set; } = default!;

  public EorIotDeviceControls Controls { get; set; } = new();
}
