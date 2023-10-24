using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Services;
using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.Iot;

public class EorPollHandler
  : JsonIotPollHandler<EorIotDeviceItem, EorPollResponse>
{
  protected override EorPollResponse MakeResponse(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem
  )
  {
    return new(
      Mode: contentItem.EorIotDevicePart.Value.Controls.Mode,
      Reset: contentItem.EorIotDevicePart.Value.Controls.ResetState
        == EorResetState.ShouldReset,
      Start: contentItem.EorIotDevicePart.Value.Controls.RunState
        == EorRunState.Started,
      Stop: contentItem.EorIotDevicePart.Value.Controls.RunState
        == EorRunState.Stopped
    );
  }

  protected override async Task<EorPollResponse> MakeResponseAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem
  )
  {
    return new(
      Mode: contentItem.EorIotDevicePart.Value.Controls.Mode,
      Reset: contentItem.EorIotDevicePart.Value.Controls.ResetState
        == EorResetState.ShouldReset,
      Start: contentItem.EorIotDevicePart.Value.Controls.RunState
        == EorRunState.Started,
      Stop: contentItem.EorIotDevicePart.Value.Controls.RunState
        == EorRunState.Stopped
    );
  }
}
