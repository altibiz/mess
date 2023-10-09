using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Updating;
using Mess.OrchardCore;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;

namespace Mess.Eor.Updating;

public class EorUpdateHandler
  : JsonMeasurementDeviceUpdateHandler<EorUpdateRequest>
{
  public const string UpdateContentType = "EorMeasurementDevice";

  public override string ContentType => UpdateContentType;

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();
    eorMeasurementDevice.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevicePart =>
      {
        eorMeasurementDevicePart.Controls.Mode = status.Mode;
        eorMeasurementDevicePart.Controls.ResetState = status.ResetState;
        eorMeasurementDevicePart.Controls.Stamp = request.Stamp;
      }
    );
    _contentManager.UpdateAsync(eorMeasurementDevice).RunSynchronously();

    _measurementClient.AddEorStatus(status);
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();
    if (
      eorMeasurementDevice.EorMeasurementDevicePart.Value.Controls.Mode
        != status.Mode
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.Controls.ResetState
        != status.ResetState
      || eorMeasurementDevice.EorMeasurementDevicePart.Value.Controls.RunState
        != status.RunState
    )
    {
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevicePart =>
        {
          eorMeasurementDevicePart.Controls.Mode = status.Mode;
          eorMeasurementDevicePart.Controls.ResetState = status.ResetState;
          eorMeasurementDevicePart.Controls.RunState = status.RunState;
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
