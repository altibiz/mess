using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Updating;
using Mess.OrchardCore;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;

namespace Mess.Eor.MeasurementDevice.Updating;

public class EorUpdateHandler
  : JsonMeasurementDeviceUpdateHandler<EorUpdateRequest>
{
  public const string UpdateContentType = "EorMeasurementDevice";

  public override string ContentType => UpdateContentType;

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();
    if (
      eorMeasurementDevice.EorMeasurementDevicePart.Value.Mode != status.Mode
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.ResetState
        != status.ResetState
    )
    {
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevicePart =>
        {
          eorMeasurementDevicePart.Mode = status.Mode;
          eorMeasurementDevicePart.ResetState = status.ResetState;
        }
      );
      _contentManager.UpdateAsync(eorMeasurementDevice).RunSynchronously();
    }

    _measurementClient.AddEorStatus(status);
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();
    if (
      eorMeasurementDevice.EorMeasurementDevicePart.Value.Mode != status.Mode
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.ResetState
        != status.ResetState
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.RunState
        != status.RunState
    )
    {
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevicePart =>
        {
          eorMeasurementDevicePart.Mode = status.Mode;
          eorMeasurementDevicePart.ResetState = status.ResetState;
          eorMeasurementDevicePart.RunState = status.RunState;
        }
      );
      await _contentManager.UpdateAsync(eorMeasurementDevice);
    }

    await _measurementClient.AddEorStatusAsync(status);
  }

  public EorUpdateHandler(
    IEorTimeseriesClient measurementClient,
    IContentManager contentManager,
    ILogger<EorUpdateHandler> logger
  )
    : base(logger)
  {
    _contentManager = contentManager;
    _measurementClient = measurementClient;
  }

  private readonly IEorTimeseriesClient _measurementClient;
  private readonly IContentManager _contentManager;
}
