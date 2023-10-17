using Mess.Eor.Abstractions.Timeseries;

public class EorIotDeviceControls
{
  public int Mode { get; set; } = 33;

  public EorMeasurementDeviceRunState RunState { get; set; } =
    EorMeasurementDeviceRunState.Stopped;

  public EorMeasurementDeviceResetState ResetState { get; set; } =
    EorMeasurementDeviceResetState.ShouldntReset;

  public int Stamp { get; set; } = 0;
}
