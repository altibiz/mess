using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.ViewModels;

public class EorIotDeviceListViewModel
{
  public List<(
    EorIotDeviceItem Item,
    EorSummary? Summary
    )> EorIotDevices { get; set; } = default!;
}
