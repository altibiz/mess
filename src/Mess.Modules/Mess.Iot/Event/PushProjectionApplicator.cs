using Mess.Event.Abstractions.Events;
using Mess.Iot.Abstractions.Client;
using Mess.Iot.Abstractions.Pushing;
using Mess.Iot.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YesSql;

namespace Mess.Iot.Event;

public class PushProjectionApplicator : IProjectionApplicator
{
  public void Apply(IServiceProvider services, IEvents events)
  {
    var client = services.GetRequiredService<ITimeseriesClient>();
    var session = services.GetRequiredService<ISession>();
    var cache =
      services.GetRequiredService<IMeasurementDeviceContentItemCache>();
    var logger = services.GetRequiredService<
      ILogger<PushProjectionApplicator>
    >();

    foreach (var @event in events.OfType<Measured>())
    {
      var contentItem = cache.Get(@event.DeviceId);
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IMeasurementDevicePushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null)
      {
        continue;
      }

      handler.Handle(
        @event.DeviceId,
        @event.Tenant,
        @event.Timestamp,
        contentItem,
        @event.Payload
      );
    }

    session.SaveChangesAsync().RunSynchronously();
  }

  public async Task ApplyAsync(
    IServiceProvider services,
    IEvents events,
    CancellationToken cancellationToken
  )
  {
    var client = services.GetRequiredService<ITimeseriesClient>();
    var session = services.GetRequiredService<ISession>();
    var cache =
      services.GetRequiredService<IMeasurementDeviceContentItemCache>();
    var logger = services.GetRequiredService<
      ILogger<PushProjectionApplicator>
    >();

    foreach (var @event in events.OfType<Measured>())
    {
      var contentItem = await cache.GetAsync(@event.DeviceId);
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IMeasurementDevicePushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null)
      {
        continue;
      }

      await handler.HandleAsync(
        @event.DeviceId,
        @event.Tenant,
        @event.Timestamp,
        contentItem,
        @event.Payload
      );
      if (cancellationToken.IsCancellationRequested)
      {
        await session.SaveChangesAsync();
        return;
      }
    }

    await session.SaveChangesAsync();
  }
}
