using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.Models;

public class EorMeasurementDeviceDataModel
{
  public EorIotDeviceControls EorMeasurementDeviceControls { get; set; } =
    default!;

  public EorSummary EorMeasurementDeviceSummary { get; set; } = default!;
}
