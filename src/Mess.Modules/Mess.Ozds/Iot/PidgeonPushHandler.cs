using Mess.Event.Abstractions.Client;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace Mess.Ozds.Iot;

public class PidgeonPushHandler
  : JsonIotPushHandler<PidgeonIotDeviceItem, PidgeonPushRequest>
{
  private readonly IIotDeviceContentItemCache _cache;

  private readonly ILogger _logger;

  private readonly IServiceProvider _services;

  private readonly IShellFeaturesManager _shellFeaturesManager;

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
                tenant,
                measurement.Timestamp,
                measurement.DeviceId,
                measurement.Data
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
                tenant,
                measurement.Timestamp,
                measurement.DeviceId,
                measurement.Data
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
}
