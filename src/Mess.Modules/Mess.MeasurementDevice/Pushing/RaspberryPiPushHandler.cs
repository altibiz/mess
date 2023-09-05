using Mess.EventStore.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Pushing;
using Mess.MeasurementDevice.Abstractions.Services;
using Mess.MeasurementDevice.EventStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;

namespace Mess.MeasurementDevice.Pushing;

public class RaspberryPiPushHandler
  : JsonMeasurementDevicePushHandler<RaspberryPiPushRequest>
{
  public override string ContentType => "RaspberryPiMeasurementDevice";

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    RaspberryPiPushRequest request
  )
  {
    var now = DateTime.UtcNow;
    var features = _shellFeaturesManager.GetEnabledFeaturesAsync().Result;
    if (features.Any(feature => feature.Id == "Mess.EventStore"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      client.RecordEvents<Measurements>(
        request.measurements
          .Select(
            measurement =>
              new Measured(
                Tenant: tenant,
                Timestamp: now,
                DeviceId: measurement.deviceId,
                Payload: measurement.request
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
          .GetServices<IMeasurementDevicePushHandler>()
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
          now,
          measurementContentItem,
          measurement.request
        );
      }
    }
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    RaspberryPiPushRequest request
  )
  {
    var now = DateTime.UtcNow;
    var features = await _shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "Mess.EventStore"))
    {
      var client = _services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Measurements>(
        request.measurements
          .Select(
            measurement =>
              new Measured(
                Tenant: tenant,
                Timestamp: now,
                DeviceId: measurement.deviceId,
                Payload: measurement.request
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
          .GetServices<IMeasurementDevicePushHandler>()
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
          now,
          measurementContentItem,
          measurement.request
        );
      }
    }
  }

  public RaspberryPiPushHandler(
    ILogger<RaspberryPiPushHandler> logger,
    IMeasurementDeviceContentItemCache cache,
    IServiceProvider services,
    IShellFeaturesManager shellFeaturesManager
  )
    : base(logger)
  {
    _cache = cache;
    _services = services;
    _shellFeaturesManager = shellFeaturesManager;
  }

  private readonly IMeasurementDeviceContentItemCache _cache;

  private readonly IServiceProvider _services;

  private readonly IShellFeaturesManager _shellFeaturesManager;
}
