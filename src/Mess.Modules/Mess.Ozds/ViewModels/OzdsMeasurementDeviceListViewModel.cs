using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.ViewModels;

public class OzdsMeasurementDeviceListViewModel
{
  public List<(
    OzdsMeasurementDeviceItem Item,
    OzdsMeasurementDeviceSummary? Summary
  )> OzdsMeasurementDevices { get; set; } = default!;
}
