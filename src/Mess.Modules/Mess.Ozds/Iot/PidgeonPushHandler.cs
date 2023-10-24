using Mess.Event.Abstractions.Client;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Abstractions.Caches;
using Mess.Ozds.Event;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using Mess.Ozds.Abstractions.Models;

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
        var measurementContentItem = _cache.Get(measurement.DeviceId);
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
          measurement.DeviceId,
          tenant,
          measurement.Timestamp,
          measurementContentItem,
          measurement.Data
        );
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
        var measurementContentItem = await _cache.GetAsync(
          measurement.DeviceId
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
          measurement.DeviceId,
          tenant,
          measurement.Timestamp,
          measurementContentItem,
          measurement.Data
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
