using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorMeasurementDeviceListViewModel
{
  public List<(
    EorMeasurementDeviceItem Item,
    EorMeasurementDeviceSummary? Summary
  )> EorMeasurementDevices { get; set; } = default!;
}
