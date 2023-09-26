using Microsoft.AspNetCore.Mvc;
using Mess.System.Extensions.Strings;
using OrchardCore.Environment.Shell;
using Microsoft.Extensions.DependencyInjection;
using Mess.Event.Abstractions.Client;
using Mess.Iot.EventStore;
using Mess.Iot.Abstractions.Updating;
using Mess.Iot.Abstractions.Pushing;
using Mess.Iot.Abstractions.Polling;
using Mess.Iot.Filters;
using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Iot.Abstractions.Services;

namespace Mess.Iot.Controllers;

[IgnoreAntiforgeryToken]
[MeasurementDeviceAuthorization]
[MeasurementDeviceAsciiResponse]
public class DeviceController : Controller
{
  [HttpPost]
  public async Task<IActionResult> Push(string deviceId)
  {
    // TODO: on each push add a timer that will notify if there was no push
    // on next push just remove the timer

    var contentItem = await _cache.GetAsync(deviceId);
    if (contentItem is null)
    {
      return NotFound($"Unknown device");
    }

    var handler = _services
      .GetServices<IMeasurementDevicePushHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      return StatusCode(500, $"Unknown handler for {contentItem.ContentType}");
    }

    var tenant = _shellSettings.GetDatabaseTablePrefix();
    var now = DateTime.UtcNow;
    var request = await Request.Body.EncodeAsync();

    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Measurements>(
        new Measured(
          Tenant: tenant,
          Timestamp: now,
          DeviceId: deviceId,
          Payload: request
        )
      );
    }
    else
    {
      var handled = await handler.HandleAsync(
        deviceId,
        tenant,
        now,
        contentItem,
        request
      );
      if (!handled)
      {
        return StatusCode(
          500,
          $"Handler for {contentItem.ContentType} returned false"
        );
      }
    }

    return Ok("pushed");
  }

  [HttpPost]
  public async Task<IActionResult> Update(string deviceId)
  {
    var contentItem = await _cache.GetAsync(deviceId);
    if (contentItem is null)
    {
      return NotFound($"Unknown device");
    }

    var handler = _services
      .GetServices<IMeasurementDeviceUpdateHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      return StatusCode(500, $"Unknown handler for {contentItem.ContentType}");
    }

    var tenant = _shellSettings.GetDatabaseTablePrefix();
    var now = DateTime.UtcNow;
    var request = await Request.Body.EncodeAsync();

    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Updates>(
        new Updated(
          Tenant: tenant,
          Timestamp: now,
          DeviceId: deviceId,
          Payload: request
        )
      );
    }
    else
    {
      var handled = await handler.HandleAsync(
        deviceId,
        tenant,
        now,
        contentItem,
        request
      );
      if (!handled)
      {
        return StatusCode(
          500,
          $"Handler for {contentItem.ContentType} returned false"
        );
      }
    }

    return Ok("updated");
  }

  [HttpGet]
  public async Task<IActionResult> Poll(string deviceId)
  {
    // TODO: on each poll add a timer that will notify if there was no poll
    // on next poll just remove the timer

    var contentItem = await _cache.GetAsync(deviceId);
    if (contentItem is null)
    {
      return NotFound("Unknown device");
    }

    var handler = _services
      .GetServices<IMeasurementDevicePollHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      return StatusCode(500, $"Unknown handler for {contentItem.ContentType}");
    }

    var tenant = _shellSettings.GetDatabaseTablePrefix();
    var now = DateTime.UtcNow;
    var response = await handler.HandleAsync(
      deviceId,
      tenant,
      now,
      contentItem
    );

    if (response is null)
    {
      return StatusCode(
        500,
        $"Handler for {contentItem.ContentType} returned null"
      );
    }

    return Ok(response);
  }

  public DeviceController(
    ShellSettings shellSettings,
    IServiceProvider services,
    IMeasurementDeviceContentItemCache cache,
    IShellFeaturesManager shellFeaturesManager
  )
  {
    _shellSettings = shellSettings;
    _services = services;
    _cache = cache;
    _shellFeaturesManager = shellFeaturesManager;
  }

  private readonly ShellSettings _shellSettings;

  private readonly IServiceProvider _services;

  private readonly IMeasurementDeviceContentItemCache _cache;

  private readonly IShellFeaturesManager _shellFeaturesManager;
}
