using Mess.Event.Abstractions.Events;
using Mess.Iot.Abstractions.Timeseries;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.Event;

public class UpdateEventDispatcher : IEventDispatcher
{
  public void Dispatch(IServiceProvider services, IEvents events)
  {
    var client = services.GetRequiredService<IIotTimeseriesClient>();
    var session = services.GetRequiredService<ISession>();
    var logger = services.GetRequiredService<ILogger<UpdateEventDispatcher>>();

    foreach (var @event in events.OfType<Updated>())
    {
      var contentItem = session
        .Query<ContentItem, IotDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync()
        .Result;
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IIotUpdateHandler>()
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
    var client = services.GetRequiredService<IIotTimeseriesClient>();
    var session = services.GetRequiredService<ISession>();
    var logger = services.GetRequiredService<ILogger<UpdateEventDispatcher>>();

    foreach (var @event in events.OfType<Updated>())
    {
      var contentItem = await session
        .Query<ContentItem, IotDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync();
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IIotUpdateHandler>()
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
