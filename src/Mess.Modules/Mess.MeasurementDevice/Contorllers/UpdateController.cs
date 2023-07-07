using Microsoft.AspNetCore.Mvc;
using Mess.System.Extensions.Strings;
using OrchardCore.Environment.Shell;
using Microsoft.Extensions.DependencyInjection;
using Mess.EventStore.Abstractions.Client;
using Mess.MeasurementDevice.EventStore;
using OrchardCore.Environment.Shell.Scope;
using YesSql;
using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Updating;
using OrchardCore.ContentManagement;
using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Controllers;

public class UpdateController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Index(
    [FromServices] IShellFeaturesManager shellFeaturesManager,
    [FromServices] IServiceProvider services,
    [FromServices] ISession session,
    [FromQuery] string deviceId,
    [FromQuery] string? handlerId
  )
  {
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
        ?.DefaultUpdateHandlerId;

      if (defaultHandlerId is null)
      {
        return BadRequest($"Unknown handler");
      }

      handlerId = defaultHandlerId;
    }

    var handler = services
      .GetServices<IUpdateHandler>()
      .FirstOrDefault(handler => handler.Id == handlerId);
    if (handler is null)
    {
      return BadRequest($"Unknown handler");
    }

    var payload = await Request.Body.EncodeAsync();

    var features = await shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.EventStore"))
    {
      var client = services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Updates>(
        new Updated(
          Tenant: ShellScope.Current.ShellContext.Settings.Name,
          Timestamp: DateTime.UtcNow,
          HandlerId: handlerId,
          DeviceId: deviceId,
          Payload: payload
        )
      );
    }
    else
    {
      var handled = handler.Handle(deviceId, contentItem, payload);
      if (!handled)
      {
        return BadRequest($"Handler {handlerId} returned false");
      }
    }

    return Ok("updated");
  }
}
