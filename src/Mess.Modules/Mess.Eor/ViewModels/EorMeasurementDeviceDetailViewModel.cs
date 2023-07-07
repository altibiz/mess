using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorMeasurementDeviceDetailViewModel
{
  public EorMeasurementDeviceItem EorMeasurementDevice { get; set; } = default!;

  public EorMeasurementDeviceSummary EorMeasurementDeviceSummary { get; set; } =
    default!;
}
