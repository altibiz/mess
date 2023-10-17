using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorMeasurementDeviceListViewModel
{
  public List<(
    EorMeasurementDeviceItem Item,
    EorSummary? Summary
  )> EorMeasurementDevices { get; set; } = default!;
}
