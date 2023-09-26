using Mess.Eor.Abstractions.Client;

namespace Mess.Eor.Models;

public class EorMeasurementDeviceDataModel
{
  public EorMeasurementDeviceControls EorMeasurementDeviceControls { get; set; } =
    default!;

  public EorMeasurementDeviceSummary EorMeasurementDeviceSummary { get; set; } =
    default!;
}
