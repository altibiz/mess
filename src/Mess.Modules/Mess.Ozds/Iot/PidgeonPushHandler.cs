using Mess.Event.Abstractions.Client;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Abstractions.Caches;
using Mess.Ozds.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;
using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.Pushing;

public class PidgeonPushHandler
  : JsonIotPushHandler<PidgeonMeasurementDeviceItem, PidgeonPushRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    PidgeonMeasurementDeviceItem contentItem,
    PidgeonPushRequest request
  )
  {
    var features = _shellFeaturesManager.GetEnabledFeaturesAsync().Result;
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      client.RecordEvents<PidgeonMeasurements>(
        request.measurements
          .Select(
            measurement =>
              new PidgeonMeasured(
                Tenant: tenant,
                Timestamp: measurement.timestamp,
                DeviceId: measurement.deviceId,
                Payload: measurement.data
              )
          )
          .ToArray()
      );
    }
    else
    {
      foreach (var measurement in request.measurements)
      {
        var measurementContentItem = _cache.Get(measurement.deviceId);
        if (measurementContentItem is null)
        {
          continue;
        }

        var measurementHandler = _services
          .GetServices<IIotPushHandler>()
          .FirstOrDefault(
            handler => handler.ContentType == measurementContentItem.ContentType
          );
        if (measurementHandler is null)
        {
          continue;
        }

        measurementHandler.Handle(
          measurement.deviceId,
          tenant,
          measurement.timestamp,
          measurementContentItem,
          measurement.data
        );
      }
    }
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    PidgeonMeasurementDeviceItem contentItem,
    PidgeonPushRequest request
  )
  {
    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.Event"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<PidgeonMeasurements>(
        request.measurements
          .Select(
            measurement =>
              new PidgeonMeasured(
                Tenant: tenant,
                Timestamp: measurement.timestamp,
                DeviceId: measurement.deviceId,
                Payload: measurement.data
              )
          )
          .ToArray()
      );
    }
    else
    {
      foreach (var measurement in request.measurements)
      {
        var measurementContentItem = await _cache.GetAsync(
          measurement.deviceId
        );
        if (measurementContentItem is null)
        {
          continue;
        }

        var measurementHandler = _services
          .GetServices<IIotPushHandler>()
          .FirstOrDefault(
            handler => handler.ContentType == measurementContentItem.ContentType
          );
        if (measurementHandler is null)
        {
          continue;
        }

        await measurementHandler.HandleAsync(
          measurement.deviceId,
          tenant,
          measurement.timestamp,
          measurementContentItem,
          measurement.data
        );
      }
    }
  }

  public PidgeonPushHandler(
    IIotDeviceContentItemCache cache,
    IServiceProvider services,
    IShellFeaturesManager shellFeaturesManager
  )
  {
    _cache = cache;
    _services = services;
    _shellFeaturesManager = shellFeaturesManager;
  }

  private readonly IIotDeviceContentItemCache _cache;

  private readonly IServiceProvider _services;

  private readonly IShellFeaturesManager _shellFeaturesManager;
}
