using Mess.Event.Abstractions.Events;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Abstractions.Caches;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YesSql;

namespace Mess.Ozds.Event;

public class PidgeonPushEventDispatcher : IEventDispatcher
{
  public void Dispatch(IServiceProvider services, IEvents events)
  {
    var session = services.GetRequiredService<ISession>();
    var cache = services.GetRequiredService<IIotDeviceContentItemCache>();
    var logger = services.GetRequiredService<
      ILogger<PidgeonPushEventDispatcher>
    >();

    foreach (var @event in events.OfType<PidgeonMeasured>())
    {
      var contentItem = cache.Get(@event.DeviceId);
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IIotPushHandler>()
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

  public async Task DispatchAsync(
    IServiceProvider services,
    IEvents events,
    CancellationToken cancellationToken
  )
  {
    var session = services.GetRequiredService<ISession>();
    var cache = services.GetRequiredService<IIotDeviceContentItemCache>();
    var logger = services.GetRequiredService<
      ILogger<PidgeonPushEventDispatcher>
    >();

    foreach (var @event in events.OfType<PidgeonMeasured>())
    {
      var contentItem = await cache.GetAsync(@event.DeviceId);
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IIotPushHandler>()
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
