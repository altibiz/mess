using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorIotDeviceListViewModel
{
  public List<(
    EorIotDeviceItem Item,
    EorSummary? Summary
  )> EorIotDevices { get; set; } = default!;
}
