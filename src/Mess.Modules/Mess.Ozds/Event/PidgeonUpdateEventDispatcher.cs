using Mess.Cms.Extensions.OrchardCore;
using Mess.Event.Abstractions.Services;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;
using YesSql;

namespace Mess.Ozds.Event;

public class PidgeonUpdateEventDispatcher : IEventDispatcher
{
  public void Dispatch(IServiceProvider services, IEvents events)
  {
    var session = services.GetRequiredService<ISession>();
    var logger = services.GetRequiredService<
      ILogger<PidgeonUpdateEventDispatcher>
    >();
    var shellSettings = services.GetRequiredService<ShellSettings>();

    foreach (var @event in events.OfType<PidgeonUpdated>())
    {
      var contentItem = session
        .Query<ContentItem, IotDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .LatestPublished()
        .FirstOrDefaultAsync()
        .Result;
      if (contentItem is null) continue;

      var handler = services
        .GetServices<IIotUpdateHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null) continue;

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
    var logger = services.GetRequiredService<
      ILogger<PidgeonUpdateEventDispatcher>
    >();
    var shellSettings = services.GetRequiredService<ShellSettings>();

    foreach (var @event in events.OfType<PidgeonUpdated>())
    {
      var contentItem = await session
        .Query<ContentItem, IotDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .LatestPublished()
        .FirstOrDefaultAsync();
      if (contentItem is null) continue;

      var handler = services
        .GetServices<IIotUpdateHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        );
      if (handler is null) continue;

      shellSettings.Name = @event.Tenant;
      await handler.HandleAsync(
        @event.DeviceId,
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
