using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Services;
using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.Iot;

public class EorPollHandler
  : JsonIotPollHandler<EorMeasurementDeviceItem, EorPollResponse>
{
  protected override EorPollResponse MakeResponse(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorMeasurementDeviceItem contentItem
  )
  {
    return new(
      Mode: contentItem.EorMeasurementDevicePart.Value.Controls.Mode,
      Reset: contentItem.EorMeasurementDevicePart.Value.Controls.ResetState
        == EorMeasurementDeviceResetState.ShouldReset,
      Start: contentItem.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Started,
      Stop: contentItem.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Stopped
    );
  }

  protected override async Task<EorPollResponse> MakeResponseAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorMeasurementDeviceItem contentItem
  )
  {
    return new(
      Mode: contentItem.EorMeasurementDevicePart.Value.Controls.Mode,
      Reset: contentItem
        .EorMeasurementDevicePart
        .Value
        .Controls
        .ResetState == EorMeasurementDeviceResetState.ShouldReset,
      Start: contentItem.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Started,
      Stop: contentItem.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Stopped
    );
  }
}
