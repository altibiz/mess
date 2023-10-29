using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.Abstractions.Models;

public class EorIotDeviceControls
{
  public int Mode { get; set; } = 33;

  public EorRunState RunState { get; set; } = EorRunState.Stopped;

  public EorResetState ResetState { get; set; } = EorResetState.ShouldntReset;

  public int Stamp { get; set; }
}
