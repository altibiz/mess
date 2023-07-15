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
using Microsoft.Extensions.Logging;
using Mess.MeasurementDevice.Abstractions.Pushing;
using Mess.MeasurementDevice.Abstractions.Polling;
using Mess.MeasurementDevice.Filters;

namespace Mess.MeasurementDevice.Controllers;

[IgnoreAntiforgeryToken]
[MeasurementDeviceAuthorization]
public class DeviceController : Controller
{
  [HttpPost]
  public async Task<IActionResult> Push(string deviceId)
  {
    // TODO: on each push add a timer that will notify if there was no push
    // on next push just remove the timer

    var contentItem = await _session
      .Query<ContentItem, MeasurementDeviceIndex>()
      .Where(index => index.DeviceId == deviceId)
      .FirstOrDefaultAsync();
    if (contentItem is null)
    {
      return BadRequest($"Unknown device");
    }

    var handler = _services
      .GetServices<IMeasurementDevicePushHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      return BadRequest($"Unknown handler");
    }

    var request = await Request.Body.EncodeAsync();

    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.EventStore"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Measurements>(
        new Measured(
          Tenant: ShellScope.Current.ShellContext.Settings.Name,
          Timestamp: DateTime.UtcNow,
          ContentType: contentItem.ContentType,
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
        return BadRequest(
          $"Handler for {contentItem.ContentType} returned false"
        );
      }
    }

    return Ok("pushed");
  }

  [HttpPost]
  public async Task<IActionResult> Update(string deviceId)
  {
    var contentItem = await _session
      .Query<ContentItem, MeasurementDeviceIndex>()
      .Where(index => index.DeviceId == deviceId)
      .FirstOrDefaultAsync();
    if (contentItem is null)
    {
      return BadRequest($"Unknown device");
    }

    var handler = _services
      .GetServices<IMeasurementDeviceUpdateHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      return BadRequest($"Unknown handler");
    }

    var request = await Request.Body.EncodeAsync();

    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.EventStore"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Updates>(
        new Updated(
          Tenant: ShellScope.Current.ShellContext.Settings.Name,
          Timestamp: DateTime.UtcNow,
          ContentType: contentItem.ContentType,
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
        return BadRequest(
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

    var contentItem = await _session
      .Query<ContentItem, MeasurementDeviceIndex>()
      .Where(index => index.DeviceId == deviceId)
      .FirstOrDefaultAsync();
    if (contentItem is null)
    {
      return BadRequest($"Unknown device");
    }

    var handler = _services
      .GetServices<IMeasurementDevicePollHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      return BadRequest($"Unknown handler");
    }

    var response = await handler.HandleAsync(deviceId, contentItem);

    if (response is null)
    {
      return BadRequest($"Handler for {contentItem.ContentType} returned null");
    }

    return Ok(response);
  }

  public DeviceController(
    ShellSettings shellSettings,
    IServiceProvider services,
    ISession session,
    IShellFeaturesManager shellFeaturesManager,
    ILogger<DeviceController> logger
  )
  {
    _shellSettings = shellSettings;
    _services = services;
    _session = session;
    _shellFeaturesManager = shellFeaturesManager;
    _logger = logger;
  }

  private readonly ShellSettings _shellSettings;

  private readonly IServiceProvider _services;

  private readonly ISession _session;

  private readonly IShellFeaturesManager _shellFeaturesManager;

  private readonly ILogger _logger;
}
