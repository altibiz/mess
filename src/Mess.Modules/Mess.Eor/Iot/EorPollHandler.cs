using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Iot.Abstractions.Services;

namespace Mess.Eor.Iot;

public class EorPollHandler
  : JsonIotPollHandler<EorIotDeviceItem, EorPollResponse>
{
  protected override EorPollResponse MakeResponse(
    string deviceId,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem
  )
  {
    return new EorPollResponse(
      contentItem.EorIotDevicePart.Value.Controls.Mode,
      contentItem.EorIotDevicePart.Value.Controls.ResetState
      == EorResetState.ShouldReset,
      contentItem.EorIotDevicePart.Value.Controls.RunState
      == EorRunState.Started,
      contentItem.EorIotDevicePart.Value.Controls.RunState
      == EorRunState.Stopped
    );
  }

  protected override async Task<EorPollResponse> MakeResponseAsync(
    string deviceId,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem
  )
  {
    return new EorPollResponse(
      contentItem.EorIotDevicePart.Value.Controls.Mode,
      contentItem.EorIotDevicePart.Value.Controls.ResetState
      == EorResetState.ShouldReset,
      contentItem.EorIotDevicePart.Value.Controls.RunState
      == EorRunState.Started,
      contentItem.EorIotDevicePart.Value.Controls.RunState
      == EorRunState.Stopped
    );
  }
}
