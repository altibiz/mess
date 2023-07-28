using Mess.Eor.Abstractions.Client;

public class EorMeasurementDeviceDataModel
{
  public EorMeasurementDeviceControls EorMeasurementDeviceControls { get; set; } =
    default!;

  public EorMeasurementDeviceSummary EorMeasurementDeviceSummary { get; set; } =
    default!;
}
