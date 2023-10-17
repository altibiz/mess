using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Services;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Eor.Iot;

public class EorUpdateHandler
  : JsonIotUpdateHandler<EorIotDeviceItem, EorUpdateRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    contentItem.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevicePart =>
      {
        eorIotDevicePart.Controls.Mode = status.Mode;
        eorIotDevicePart.Controls.ResetState = status.ResetState;
        eorIotDevicePart.Controls.Stamp = request.Stamp;
      }
    );
    _contentManager.UpdateAsync(contentItem).RunSynchronously();

    _measurementClient.AddEorStatus(status);
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(tenant, deviceId);

    if (
      contentItem.EorIotDevicePart.Value.Controls.Mode != status.Mode
      || contentItem.EorIotDevicePart.Value.Controls.ResetState
        != status.ResetState
      || contentItem.EorIotDevicePart.Value.Controls.RunState
        != status.RunState
    )
    {
      contentItem.Alter(
        eorIotDevice => eorIotDevice.EorIotDevicePart,
        eorIotDevicePart =>
        {
          eorIotDevicePart.Controls.Mode = status.Mode;
          eorIotDevicePart.Controls.ResetState = status.ResetState;
          eorIotDevicePart.Controls.RunState = status.RunState;
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
