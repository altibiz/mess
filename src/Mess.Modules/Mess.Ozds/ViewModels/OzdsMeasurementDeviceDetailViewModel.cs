using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.ViewModels;

public class OzdsMeasurementDeviceDetailViewModel
{
  public OzdsMeasurementDeviceItem EorMeasurementDevice { get; set; } =
    default!;

  public OzdsMeasurementDeviceSummary EorMeasurementDeviceSummary { get; set; } =
    default!;
}
