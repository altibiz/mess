using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Services;
using Mess.OrchardCore;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;

namespace Mess.Eor.Iot;

public class EorUpdateHandler
  : JsonIotUpdateHandler<EorMeasurementDeviceItem, EorUpdateRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorMeasurementDeviceItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    contentItem.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevicePart =>
      {
        eorMeasurementDevicePart.Controls.Mode = status.Mode;
        eorMeasurementDevicePart.Controls.ResetState = status.ResetState;
        eorMeasurementDevicePart.Controls.Stamp = request.Stamp;
      }
    );
    _contentManager.UpdateAsync(contentItem).RunSynchronously();

    _measurementClient.AddEorStatus(status);
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorMeasurementDeviceItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    if (
      contentItem.EorMeasurementDevicePart.Value.Controls.Mode != status.Mode
      || contentItem.EorMeasurementDevicePart.Value.Controls.ResetState
        != status.ResetState
      || contentItem.EorMeasurementDevicePart.Value.Controls.RunState
        != status.RunState
    )
    {
      contentItem.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevicePart =>
        {
          eorMeasurementDevicePart.Controls.Mode = status.Mode;
          eorMeasurementDevicePart.Controls.ResetState = status.ResetState;
          eorMeasurementDevicePart.Controls.RunState = status.RunState;
        }
      );
      await _contentManager.UpdateAsync(contentItem);
    }

    await _measurementClient.AddEorStatusAsync(status);
  }

  public EorUpdateHandler(
    IEorTimeseriesClient measurementClient,
    IContentManager contentManager
  )
  {
    _contentManager = contentManager;
    _measurementClient = measurementClient;
  }

  private readonly IEorTimeseriesClient _measurementClient;

  private readonly IContentManager _contentManager;
}
