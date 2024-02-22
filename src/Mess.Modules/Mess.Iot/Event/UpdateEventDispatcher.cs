using Mess.Cms.Extensions.OrchardCore;
using Mess.Event.Abstractions.Services;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;
using YesSql;

namespace Mess.Iot.Event;

public class UpdateEventDispatcher : IEventDispatcher
{
  public void Dispatch(IServiceProvider services, IEvents events)
  {
    var session = services.GetRequiredService<ISession>();
    var logger = services.GetRequiredService<ILogger<UpdateEventDispatcher>>();
    var shellSettings = services.GetRequiredService<ShellSettings>();

    foreach (var @event in events.OfType<Updated>())
    {
      var contentItem = session
        .Query<ContentItem, IotDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync()
        .Result;
      if (contentItem is null)
      {
        logger.LogError(
          "Content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }

      var handler = services
        .GetServices<IIotUpdateHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null)
      {
        logger.LogError(
          "Update handler for content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }


      shellSettings.Name = @event.Tenant;
      handler.Handle(
        @event.DeviceId,
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
    var logger = services.GetRequiredService<ILogger<UpdateEventDispatcher>>();
    var shellSettings = services.GetRequiredService<ShellSettings>();

    foreach (var @event in events.OfType<Updated>())
    {
      var contentItem = await session
        .Query<ContentItem, IotDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync();
      if (contentItem is null)
      {
        logger.LogError(
          "Content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }

      var handler = services
        .GetServices<IIotUpdateHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null)
      {
        logger.LogError(
          "Update handler for content item with device id {} not found",
          @event.DeviceId
        );
        continue;
      }

      try
      {
        shellSettings.Name = @event.Tenant;
        await handler.HandleAsync(
          @event.DeviceId,
          @event.Timestamp,
          contentItem,
          @event.Payload
        );
      }
      catch (Exception exception)
      {
        logger.LogError(
          exception,
          "Error while handling update event for device {}",
          @event.DeviceId
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
