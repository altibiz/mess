using Mess.Cms;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Iot.Abstractions.Services;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;

namespace Mess.Eor.Iot;

public class EorUpdateHandler
  : JsonIotUpdateHandler<EorIotDeviceItem, EorUpdateRequest>
{
  private readonly IContentManager _contentManager;

  private readonly IEorTimeseriesClient _measurementClient;

  private readonly ShellSettings _shellSettings;

  public EorUpdateHandler(
    IEorTimeseriesClient measurementClient,
    IContentManager contentManager,
    ShellSettings shellSettings
  )
  {
    _contentManager = contentManager;
    _measurementClient = measurementClient;
    _shellSettings = shellSettings;
  }

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(_shellSettings.GetDatabaseTablePrefix(), deviceId);

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
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorUpdateRequest request
  )
  {
    var status = request.ToStatus(_shellSettings.GetDatabaseTablePrefix(), deviceId);

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
      await _contentManager.PublishAsync(contentItem);
    }

    await _measurementClient.AddEorStatusAsync(status);
  }
}
