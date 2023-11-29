using Mess.Event.Abstractions.Services;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Abstractions.Caches;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YesSql;

namespace Mess.Iot.Event;

public class PushEventDispatcher : IEventDispatcher
{
  public void Dispatch(IServiceProvider services, IEvents events)
  {
    var session = services.GetRequiredService<ISession>();
    var cache = services.GetRequiredService<IIotDeviceContentItemCache>();
    var logger = services.GetRequiredService<ILogger<PushEventDispatcher>>();

    foreach (var @event in events.OfType<Measured>())
    {
      var contentItem = cache.GetIotDevice(@event.DeviceId);
      if (contentItem is null)
      {
        logger.LogError(
          "Content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }

      var handler = services
        .GetServices<IIotPushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null)
      {
        logger.LogError(
          "Push handler for content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }

      try
      {
        handler.Handle(
          @event.DeviceId,
          @event.Tenant,
          @event.Timestamp,
          contentItem,
          @event.Payload
        );
      }
      catch (Exception exception)
      {
        logger.LogError(
          exception,
          "Error while dispatching events of {}",
          handler.GetType().Name
        );
      }
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
    var logger = services.GetRequiredService<ILogger<PushEventDispatcher>>();

    foreach (var @event in events.OfType<Measured>())
    {
      var contentItem = await cache.GetIotDeviceAsync(@event.DeviceId);
      if (contentItem is null)
      {
        logger.LogError(
          "Content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }

      var handler = services
        .GetServices<IIotPushHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null)
      {
        logger.LogError(
         "Push handler for content item with device id {} not found",
         @event.DeviceId
        );
        continue;
      }

      try
      {
        await handler.HandleAsync(
          @event.DeviceId,
          @event.Tenant,
          @event.Timestamp,
          contentItem,
          @event.Payload
        );
      }
      catch (Exception exception)
      {
        logger.LogError(
          exception,
          "Error while dispatching events of {}",
          handler.GetType().Name
        );
      }

      if (cancellationToken.IsCancellationRequested)
      {
        await session.SaveChangesAsync();
        return;
      }
    }

    await session.SaveChangesAsync();
  }
}
