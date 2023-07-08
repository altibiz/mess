using Mess.Eor.Abstractions.Models;
using Mess.Eor.MeasurementDevice.Pushing;
using Mess.Eor.MeasurementDevice.Updating;
using Mess.MeasurementDevice.Abstractions.Models;
using Msss.Eor.MeasurementDevice.Polling;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

namespace Mess.Eor.MeasurementDevice.Handlers;

public class EorMeasurementDevicePartHandler
  : ContentPartHandler<MeasurementDevicePart>
{
  public override Task ActivatedAsync(
    ActivatedContentContext context,
    MeasurementDevicePart part
  )
  {
    var contentItem = context.ContentItem;

    if (contentItem.ContentType == EorMeasurementDeviceItem.ContentType)
    {
      part.PushHandlerId ??= EorPushHandler.PushHandlerId;
      part.PollHandlerId ??= EorPollHandler.PollHandlerId;
      part.UpdateHandlerId ??= EorStatusHandler.UpdateHandlerId;
    }

    part.Apply();

    return Task.CompletedTask;
  }
}
