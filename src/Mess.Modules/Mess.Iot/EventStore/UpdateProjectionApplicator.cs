using Mess.EventStore.Abstractions.Events;
using Mess.Iot.Abstractions.Client;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Updating;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.EventStore;

public class UpdateProjectionApplicator : IProjectionApplicator
{
  public void Apply(IServiceProvider services, IEvents events)
  {
    var client = services.GetRequiredService<ITimeseriesClient>();
    var session = services.GetRequiredService<ISession>();
    var logger = services.GetRequiredService<
      ILogger<UpdateProjectionApplicator>
    >();

    foreach (var @event in events.OfType<Updated>())
    {
      var contentItem = session
        .Query<ContentItem, MeasurementDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync()
        .Result;
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IMeasurementDeviceUpdateHandler>()
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
    var logger = services.GetRequiredService<
      ILogger<UpdateProjectionApplicator>
    >();

    foreach (var @event in events.OfType<Updated>())
    {
      var contentItem = await session
        .Query<ContentItem, MeasurementDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync();
      if (contentItem is null)
      {
        continue;
      }

      var handler = services
        .GetServices<IMeasurementDeviceUpdateHandler>()
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
