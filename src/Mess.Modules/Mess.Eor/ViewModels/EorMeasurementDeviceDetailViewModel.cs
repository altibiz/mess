using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.ViewModels;

public class EorMeasurementDeviceDetailViewModel
{
  public EorMeasurementDeviceItem EorMeasurementDevice { get; set; } = default!;

  public EorSummary EorMeasurementDeviceSummary { get; set; } = default!;
}
