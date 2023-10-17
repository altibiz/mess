using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorIotDeviceDetailViewModel
{
  public EorIotDeviceItem EorIotDeviceItem { get; set; } = default!;

  public EorSummary EorSummary { get; set; } = default!;
}
