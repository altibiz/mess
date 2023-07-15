using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Updating;
using Mess.OrchardCore;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;

namespace Mess.Eor.MeasurementDevice.Updating;

public class EorStatusHandler : JsonMeasurementDeviceUpdateHandler<EorStatus>
{
  public const string UpdateContentType = "EorMeasurementDevice";

  public override string ContentType => UpdateContentType;

  protected override void Handle(
    string deviceId,
    ContentItem contentItem,
    EorStatus request
  )
  {
    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();
    if (
      eorMeasurementDevice.EorMeasurementDevicePart.Value.Mode != request.Mode
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.ResetState
        != request.ResetState
    )
    {
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevicePart =>
        {
          eorMeasurementDevicePart.Mode = request.Mode;
          eorMeasurementDevicePart.ResetState = request.ResetState;
        }
      );
      _contentManager.PublishAsync(eorMeasurementDevice).RunSynchronously();
    }

    _measurementClient.AddEorStatus(request);
  }

  protected override async Task HandleAsync(
    string deviceId,
    ContentItem contentItem,
    EorStatus request
  )
  {
    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();
    if (
      eorMeasurementDevice.EorMeasurementDevicePart.Value.Mode != request.Mode
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.ResetState
        != request.ResetState
    )
    {
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevicePart =>
        {
          eorMeasurementDevicePart.Mode = request.Mode;
          eorMeasurementDevicePart.ResetState = request.ResetState;
        }
      );
      await _contentManager.PublishAsync(eorMeasurementDevice);
    }

    await _measurementClient.AddEorStatusAsync(request);
  }

  public EorStatusHandler(
    IEorTimeseriesClient measurementClient,
    IContentManager contentManager,
    ILogger<EorStatusHandler> logger
  )
    : base(logger)
  {
    _contentManager = contentManager;
    _measurementClient = measurementClient;
  }

  private readonly IEorTimeseriesClient _measurementClient;
  private readonly IContentManager _contentManager;
}
