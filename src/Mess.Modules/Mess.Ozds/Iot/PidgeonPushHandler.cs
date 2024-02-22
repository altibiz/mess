using Mess.Cms.Extensions.Objects;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Event.Abstractions.Client;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;

namespace Mess.Ozds.Iot;

public class PidgeonPushHandler
  : JsonIotPushHandler<PidgeonIotDeviceItem, PidgeonPushRequest>
{
  private readonly IIotDeviceContentItemCache _cache;

  private readonly ILogger _logger;

  private readonly IServiceProvider _services;

  private readonly IShellFeaturesManager _shellFeaturesManager;

  private readonly ShellSettings _shellSettings;

  public PidgeonPushHandler(
    IIotDeviceContentItemCache cache,
    IServiceProvider services,
    IShellFeaturesManager shellFeaturesManager,
    ILogger<PidgeonPushHandler> logger,
    ShellSettings shellSettings
  )
  {
    _cache = cache;
    _services = services;
    _shellFeaturesManager = shellFeaturesManager;
    _logger = logger;
    _shellSettings = shellSettings;
  }

  private record PropagatedBulkRequest(
    string DeviceId,
    DateTimeOffset Timestamp,
    ContentItem Item,
    string Request,
    IIotPushHandler Handler
  );

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    PidgeonIotDeviceItem contentItem,
    PidgeonPushRequest request
  )
  {
    var features = _shellFeaturesManager.GetEnabledFeaturesAsync().Result;
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      client.RecordEvents<PidgeonMeasurements>(
        request.Measurements
          .Select(
            measurement =>
              new PidgeonMeasured(
                _shellSettings.GetTenantName(),
                measurement.Timestamp,
                measurement.DeviceId,
                measurement.Data.ToString()
              )
          )
          .ToArray()
      );
      return;
    }

    var propagatedRequests = new Dictionary<IIotPushHandler, List<BulkIotPushRequest>>();
    foreach (var measurement in request.Measurements)
    {
      var measurementContentItem = _cache.GetIotDevice(measurement.DeviceId);
      if (measurementContentItem is null)
      {
        _logger.LogError(
          "Content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var measurementHandler = _services
        .GetServices<IIotPushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == measurementContentItem.ContentType
        );
      if (measurementHandler is null)
      {
        _logger.LogError(
          "Push handler for content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var handlerRequest = new BulkIotPushRequest(
          measurement.DeviceId,
          measurement.Timestamp,
          measurementContentItem,
          measurement.Data.ToString()
      );
      if (propagatedRequests.TryGetValue(measurementHandler, out var handlerRequests))
      {
        handlerRequests.Add(handlerRequest);
      }
      else
      {
        propagatedRequests.Add(measurementHandler, new() { handlerRequest });
      }
    }

    foreach (var (handler, requests) in propagatedRequests)
    {
      try
      {
        handler.HandleBulk(requests.ToArray());
      }
      catch (Exception exception)
      {
        _logger.LogError(
          exception,
          "Error while handling push event for handler {}",
          handler.GetType().Name
        );
      }
    }
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    PidgeonIotDeviceItem contentItem,
    PidgeonPushRequest request
  )
  {
    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<PidgeonMeasurements>(
        request.Measurements
          .Select(
            measurement =>
              new PidgeonMeasured(
                _shellSettings.GetTenantName(),
                measurement.Timestamp,
                measurement.DeviceId,
                measurement.Data.ToString()
              )
          )
          .ToArray()
      );
      return;
    }
    var propagatedRequests = new Dictionary<IIotPushHandler, List<BulkIotPushRequest>>();
    foreach (var measurement in request.Measurements)
    {
      var measurementContentItem = _cache.GetIotDevice(measurement.DeviceId);
      if (measurementContentItem is null)
      {
        _logger.LogError(
          "Content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var measurementHandler = _services
        .GetServices<IIotPushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == measurementContentItem.ContentType
        );
      if (measurementHandler is null)
      {
        _logger.LogError(
          "Push handler for content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var handlerRequest = new BulkIotPushRequest(
          measurement.DeviceId,
          measurement.Timestamp,
          measurementContentItem,
          measurement.Data.ToString()
      );
      if (propagatedRequests.TryGetValue(measurementHandler, out var handlerRequests))
      {
        handlerRequests.Add(handlerRequest);
      }
      else
      {
        propagatedRequests.Add(measurementHandler, new() { handlerRequest });
      }
    }

    foreach (var (handler, requests) in propagatedRequests)
    {
      try
      {
        await handler.HandleBulkAsync(requests.ToArray());
      }
      catch (Exception exception)
      {
        _logger.LogError(
          exception,
          "Error while handling push event for handler {}",
          handler.GetType().Name
        );
      }
    }
  }

  protected override void HandleBulk(BulkIotJsonPushRequest<PidgeonIotDeviceItem, PidgeonPushRequest>[] requests)
  {
    var features = _shellFeaturesManager.GetEnabledFeaturesAsync().Result;
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      client.RecordEvents<PidgeonMeasurements>(
        requests
          .SelectMany(
            request =>
              request.Request.Measurements.Select(measurement =>
                new PidgeonMeasured(
                  _shellSettings.GetTenantName(),
                  measurement.Timestamp,
                  measurement.DeviceId,
                  measurement.Data.ToString()
                )
              )
          )
          .ToArray()
      );
      return;
    }

    var propagatedRequests = new Dictionary<IIotPushHandler, List<BulkIotPushRequest>>();
    foreach (var measurement in requests
      .SelectMany(request =>
        request.Request.Measurements))
    {
      var measurementContentItem = _cache.GetIotDevice(measurement.DeviceId);
      if (measurementContentItem is null)
      {
        _logger.LogError(
          "Content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var measurementHandler = _services
        .GetServices<IIotPushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == measurementContentItem.ContentType
        );
      if (measurementHandler is null)
      {
        _logger.LogError(
          "Push handler for content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var handlerRequest = new BulkIotPushRequest(
          measurement.DeviceId,
          measurement.Timestamp,
          measurementContentItem,
          measurement.Data.ToString()
      );
      if (propagatedRequests.TryGetValue(measurementHandler, out var handlerRequests))
      {
        handlerRequests.Add(handlerRequest);
      }
      else
      {
        propagatedRequests.Add(measurementHandler, new() { handlerRequest });
      }
    }

    foreach (var (handler, handlerRequests) in propagatedRequests)
    {
      try
      {
        handler.HandleBulk(handlerRequests.ToArray());
      }
      catch (Exception exception)
      {
        _logger.LogError(
          exception,
          "Error while handling push event for handler {}",
          handler.GetType().Name
        );
      }
    }
  }

  protected override async Task HandleBulkAsync(BulkIotJsonPushRequest<PidgeonIotDeviceItem, PidgeonPushRequest>[] requests)
  {
    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<PidgeonMeasurements>(
        requests
          .SelectMany(
            request =>
              request.Request.Measurements.Select(measurement =>
                new PidgeonMeasured(
                  _shellSettings.GetTenantName(),
                  measurement.Timestamp,
                  measurement.DeviceId,
                  measurement.Data.ToString()
                )
              )
          )
          .ToArray()
      );
      return;
    }
    var propagatedRequests = new Dictionary<IIotPushHandler, List<BulkIotPushRequest>>();
    foreach (var measurement in requests
      .SelectMany(request =>
        request.Request.Measurements))
    {
      var measurementContentItem = _cache.GetIotDevice(measurement.DeviceId);
      if (measurementContentItem is null)
      {
        _logger.LogError(
          "Content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var measurementHandler = _services
        .GetServices<IIotPushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == measurementContentItem.ContentType
        );
      if (measurementHandler is null)
      {
        _logger.LogError(
          "Push handler for content item with device id {} not found",
          measurement.DeviceId
        );
        continue;
      }

      var handlerRequest = new BulkIotPushRequest(
          measurement.DeviceId,
          measurement.Timestamp,
          measurementContentItem,
          measurement.Data.ToString()
      );
      if (propagatedRequests.TryGetValue(measurementHandler, out var handlerRequests))
      {
        handlerRequests.Add(handlerRequest);
      }
      else
      {
        propagatedRequests.Add(measurementHandler, new() { handlerRequest });
      }
    }

    foreach (var (handler, handlerRequests) in propagatedRequests)
    {
      try
      {
        await handler.HandleBulkAsync(handlerRequests.ToArray());
      }
      catch (Exception exception)
      {
        _logger.LogError(
          exception,
          "Error while handling push event for handler {}",
          handler.GetType().Name
        );
      }
    }
  }
}
