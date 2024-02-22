using Mess.Cms.Extensions.OrchardCore;
using Mess.Event.Abstractions.Client;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Event;
using Mess.Iot.Filters;
using Mess.Prelude.Extensions.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;

namespace Mess.Iot.Controllers;

[IgnoreAntiforgeryToken]
[IotDeviceAuthorization]
[IotDeviceAsciiResponse]
public class IotDeviceController : Controller
{
  private readonly IIotDeviceContentItemCache _cache;

  private readonly IServiceProvider _services;

  private readonly IShellFeaturesManager _shellFeaturesManager;

  private readonly ShellSettings _shellSettings;

  public IotDeviceController(
    ShellSettings shellSettings,
    IServiceProvider services,
    IIotDeviceContentItemCache cache,
    IShellFeaturesManager shellFeaturesManager
  )
  {
    _shellSettings = shellSettings;
    _services = services;
    _cache = cache;
    _shellFeaturesManager = shellFeaturesManager;
  }

  [HttpPost]
  public async Task<IActionResult> Push(string deviceId)
  {
    // TODO: on each push add a timer that will notify if there was no push
    // on next push just remove the timer

    var contentItem = await _cache.GetIotDeviceAsync(deviceId);
    if (contentItem is null) return NotFound("Unknown device");

    var handler = _services
      .GetServices<IIotPushHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
      return StatusCode(500, $"Unknown handler for {contentItem.ContentType}");

    var tenant = _shellSettings.GetDatabaseTablePrefix();
    var now = DateTimeOffset.UtcNow;
    var request = await Request.Body.EncodeAsync();

    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Measurements>(
        new Measured(
          tenant,
          now,
          deviceId,
          request
        )
      );
    }
    else
    {
      await handler.HandleAsync(deviceId, now, contentItem, request);
    }

    return Ok("pushed");
  }

  [HttpPost]
  public async Task<IActionResult> Update(string deviceId)
  {
    var contentItem = await _cache.GetIotDeviceAsync(deviceId);
    if (contentItem is null) return NotFound("Unknown device");

    var handler = _services
      .GetServices<IIotUpdateHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
      return StatusCode(500, $"Unknown handler for {contentItem.ContentType}");

    var tenant = _shellSettings.GetDatabaseTablePrefix();
    var now = DateTimeOffset.UtcNow;
    var request = await Request.Body.EncodeAsync();

    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Updates>(
        new Updated(
          tenant,
          now,
          deviceId,
          request
        )
      );
    }
    else
    {
      await handler.HandleAsync(deviceId, now, contentItem, request);
    }

    return Ok("updated");
  }

  [HttpGet]
  public async Task<IActionResult> Poll(string deviceId)
  {
    // TODO: on each poll add a timer that will notify if there was no poll
    // on next poll just remove the timer

    var contentItem = await _cache.GetIotDeviceAsync(deviceId);
    if (contentItem is null) return NotFound("Unknown device");

    var handler = _services
      .GetServices<IIotPollHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
      return StatusCode(500, $"Unknown handler for {contentItem.ContentType}");

    var tenant = _shellSettings.GetDatabaseTablePrefix();
    var now = DateTimeOffset.UtcNow;
    var response = await handler.HandleAsync(
      deviceId,
      now,
      contentItem
    );

    return response is null
      ? StatusCode(
        500,
        $"Handler for {contentItem.ContentType} returned null"
      )
      : (IActionResult)Ok(response);
  }
}
