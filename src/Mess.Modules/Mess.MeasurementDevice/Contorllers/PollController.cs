using Microsoft.AspNetCore.Mvc;
using Mess.MeasurementDevice.Abstractions.Polling;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using YesSql;
using Mess.MeasurementDevice.Abstractions.Indexes;
using OrchardCore.ContentManagement;
using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Controllers;

public class PollController : Controller
{
  [HttpGet]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Index(
    [FromServices] ShellSettings shellSettings,
    [FromServices] IServiceProvider services,
    [FromServices] ISession session,
    [FromQuery] string deviceId,
    [FromQuery] string? handlerId
  )
  {
    // TODO: on each poll add a timer that will notify if there was no poll
    // on next poll just remove the timer

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
        ?.DefaultPollHandlerId;

      if (defaultHandlerId is null)
      {
        return BadRequest($"Unknown handler");
      }

      handlerId = defaultHandlerId;
    }

    var handler = services
      .GetServices<IPollHandler>()
      .FirstOrDefault(handler => handler.Id == handlerId);
    if (handler is null)
    {
      return BadRequest($"Unknown handler");
    }

    var response = await handler.HandleAsync(deviceId, contentItem);

    if (response is null)
    {
      return BadRequest($"Handler {handlerId} returned null");
    }

    return Ok(response);
  }
}
