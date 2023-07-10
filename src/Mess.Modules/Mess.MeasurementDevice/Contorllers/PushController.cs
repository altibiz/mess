using Microsoft.AspNetCore.Mvc;
using Mess.System.Extensions.Strings;
using Mess.MeasurementDevice.Abstractions.Pushing;
using OrchardCore.Environment.Shell;
using Microsoft.Extensions.DependencyInjection;
using Mess.EventStore.Abstractions.Client;
using Mess.MeasurementDevice.EventStore;
using OrchardCore.Environment.Shell.Scope;
using YesSql;
using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Models;
using OrchardCore.ContentManagement;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.Controllers;

public class PushController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Index(
    [FromServices] IShellFeaturesManager shellFeaturesManager,
    [FromServices] IServiceProvider services,
    [FromServices] ISession session,
    [FromServices] ILogger<PushController> logger,
    [FromQuery] string deviceId,
    [FromQuery] string? handlerId
  )
  {
    // TODO: on each push add a timer that will notify if there was no push
    // on next push just remove the timer

    logger.LogInformation(
      "Push from {deviceId} to {handlerId}: {body}",
      deviceId,
      handlerId,
      await Request.Body.EncodeAsync()
    );

    var contentItem = await session
      .Query<ContentItem, MeasurementDeviceIndex>()
      .Where(index => index.DeviceId == deviceId)
      .FirstOrDefaultAsync();
    if (contentItem is null)
    {
      return BadRequest($"Unknown device");
    }

    if (handlerId is null)
    {
      var defaultHandlerId = contentItem
        .As<MeasurementDevicePart>()
        ?.DefaultPushHandlerId;

      if (defaultHandlerId is null)
      {
        return BadRequest($"Unknown handler");
      }

      handlerId = defaultHandlerId;
    }

    var handler = services
      .GetServices<IPushHandler>()
      .FirstOrDefault(handler => handler.Id == handlerId);
    if (handler is null)
    {
      return BadRequest($"Unknown handler");
    }

    var request = await Request.Body.EncodeAsync();

    var features = await shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.EventStore"))
    {
      var client = services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Measurements>(
        new Measured(
          Tenant: ShellScope.Current.ShellContext.Settings.Name,
          Timestamp: DateTime.UtcNow,
          HandlerId: handlerId,
          DeviceId: deviceId,
          Payload: request
        )
      );
    }
    else
    {
      var handled = handler.Handle(deviceId, contentItem, request);
      if (!handled)
      {
        return BadRequest($"Handler {handlerId} returned false");
      }
    }

    return Ok("measured");
  }
}
