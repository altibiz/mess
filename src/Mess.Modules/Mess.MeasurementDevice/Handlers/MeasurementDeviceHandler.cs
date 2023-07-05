using Mess.MeasurementDevice.Abstractions.Models;
using Mess.MeasurementDevice.Pushing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

namespace Mess.MeasurementDevice.Handlers;

public class MeasurementDevicePartHandler
  : ContentPartHandler<MeasurementDevicePart>
{
  public override Task ActivatedAsync(
    ActivatedContentContext context,
    MeasurementDevicePart part
  )
  {
    var contentItem = context.ContentItem;

    if (contentItem.ContentType == EgaugeMeasurementDeviceItem.ContentType)
    {
      part.DefaultPushHandlerId ??= EgaugePushHandler.PushHandlerId;
    }

    part.Apply();

    return Task.CompletedTask;
  }
}
