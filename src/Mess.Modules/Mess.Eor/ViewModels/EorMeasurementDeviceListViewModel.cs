using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorMeasurementDeviceListViewModel
{
  public List<EorMeasurementDeviceItem> EorMeasurementDevices { get; set; } =
    default!;

  public Dictionary<
    string,
    EorMeasurementDeviceSummary
  > EorMeasurementDeviceSummaries { get; set; } = default!;
}
