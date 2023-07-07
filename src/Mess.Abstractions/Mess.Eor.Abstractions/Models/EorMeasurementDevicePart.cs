using Mess.Eor.Abstractions.Client;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Models;

public class EorMeasurementDevicePart : ContentPart
{
  public UserPickerField Owner { get; set; } = default!;

  public DateField ManufactureDate { get; set; } = default!;

  public TextField Manufacturer { get; set; } = default!;

  public DateField CommisionDate { get; set; } = default!;

  public TextField ProductNumber { get; set; } = default!;

  public NumericField Latitude { get; set; } = default!;

  public NumericField Longitude { get; set; } = default!;

  public int Mode { get; set; } = 0;

  public EorMeasurementDeviceRunState RunState { get; set; } =
    EorMeasurementDeviceRunState.Stopped;

  public EorMeasurementDeviceResetState ResetState { get; set; } =
    EorMeasurementDeviceResetState.ShouldntReset;
}
