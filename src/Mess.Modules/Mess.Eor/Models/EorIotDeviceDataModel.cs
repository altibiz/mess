using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.Models;

public class EorIotDeviceDataModel
{
  public EorIotDeviceControls Controls { get; set; } = default!;

  public EorSummary Summary { get; set; } = default!;
}
