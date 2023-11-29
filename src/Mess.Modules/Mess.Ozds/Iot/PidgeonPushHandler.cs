using Mess.Event.Abstractions.Client;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Abstractions.Caches;
using Mess.Ozds.Event;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using Mess.Ozds.Abstractions.Models;
using Microsoft.Extensions.Logging;

namespace Mess.Ozds.Iot;

public class PidgeonPushHandler
  : JsonIotPushHandler<PidgeonIotDeviceItem, PidgeonPushRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
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
                Tenant: tenant,
                Timestamp: measurement.Timestamp,
                DeviceId: measurement.DeviceId,
                Payload: measurement.Data
              )
          )
          .ToArray()
      );
    }
    else
    {
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

        try
        {
          measurementHandler.Handle(
            measurement.DeviceId,
            tenant,
            measurement.Timestamp,
            measurementContentItem,
            measurement.Data
          );
        }
        catch (Exception exception)
        {
          _logger.LogError(
            exception,
            "Error while handling push event for device {}",
            measurement.DeviceId
          );
        }
      }
    }
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
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
                Tenant: tenant,
                Timestamp: measurement.Timestamp,
                DeviceId: measurement.DeviceId,
                Payload: measurement.Data
              )
          )
          .ToArray()
      );
    }
    else
    {
      foreach (var measurement in request.Measurements)
      {
        var measurementContentItem = await _cache.GetIotDeviceAsync(
          measurement.DeviceId
        );
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

        try
        {
          await measurementHandler.HandleAsync(
            measurement.DeviceId,
            tenant,
            measurement.Timestamp,
            measurementContentItem,
            measurement.Data
          );
        }
        catch (Exception exception)
        {
          _logger.LogError(
            exception,
            "Error while handling push event for device {}",
            measurement.DeviceId
          );
        }
      }
    }
  }

  public PidgeonPushHandler(
    IIotDeviceContentItemCache cache,
    IServiceProvider services,
    IShellFeaturesManager shellFeaturesManager,
    ILogger<PidgeonPushHandler> logger
  )
  {
    _cache = cache;
    _services = services;
    _shellFeaturesManager = shellFeaturesManager;
    _logger = logger;
  }

  private readonly IIotDeviceContentItemCache _cache;

  private readonly IServiceProvider _services;

  private readonly IShellFeaturesManager _shellFeaturesManager;

  private readonly ILogger _logger;
}
