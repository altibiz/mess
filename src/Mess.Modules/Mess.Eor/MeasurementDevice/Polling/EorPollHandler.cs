using Mess.Eor.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Polling;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;
using YesSql;
using Microsoft.Extensions.Logging;
using Mess.Eor.MeasurementDevice.Polling;
using Mess.Eor.Abstractions.Client;

namespace Mess.Eor.MeasurementDevice.Polling;

public class EorPollHandler : JsonMeasurementDevicePollHandler<EorPollResponse>
{
  public const string PollContentType = "EorMeasurementDevice";

  public override string ContentType => PollContentType;

  protected override EorPollResponse MakeResponse(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem
  )
  {
    var measurementDevice = contentItem.AsContent<EorMeasurementDeviceItem>();
    if (measurementDevice is null)
    {
      throw new ArgumentException($"Measurement device {deviceId} not found");
    }

    return new(
      Mode: measurementDevice.EorMeasurementDevicePart.Value.Controls.Mode,
      Reset: measurementDevice
        .EorMeasurementDevicePart
        .Value
        .Controls
        .ResetState == EorMeasurementDeviceResetState.ShouldReset,
      Start: measurementDevice.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Started,
      Stop: measurementDevice.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Stopped
    );
  }

  protected override async Task<EorPollResponse> MakeResponseAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem
  )
  {
    var measurementDevice = contentItem.AsContent<EorMeasurementDeviceItem>();
    if (measurementDevice is null)
    {
      throw new ArgumentException($"Measurement device {deviceId} not found");
    }

    return new(
      Mode: measurementDevice.EorMeasurementDevicePart.Value.Controls.Mode,
      Reset: measurementDevice
        .EorMeasurementDevicePart
        .Value
        .Controls
        .ResetState == EorMeasurementDeviceResetState.ShouldReset,
      Start: measurementDevice.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Started,
      Stop: measurementDevice.EorMeasurementDevicePart.Value.Controls.RunState
        == EorMeasurementDeviceRunState.Stopped
    );
  }

  public EorPollHandler(ILogger<EorPollHandler> logger, ISession session)
    : base(logger)
  {
    _session = session;
  }

  private readonly ISession _session;
}
