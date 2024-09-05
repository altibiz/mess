using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.ViewModels;

public class EorIotDeviceDetailViewModel
{
  public EorIotDeviceItem EorIotDeviceItem { get; set; } = default!;
  public EorSummary EorSummary { get; set; } = default!;
  public string? Culture { get; set; }
}
