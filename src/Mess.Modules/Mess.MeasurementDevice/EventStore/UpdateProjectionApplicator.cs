using Mess.EventStore.Abstractions.Events;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Updating;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.MeasurementDevice.EventStore;

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
      var handler = services
        .GetServices<IUpdateHandler>()
        .FirstOrDefault(handler => handler.Id == @event.HandlerId);
      if (handler is null)
      {
        continue;
      }

      var contentItem = session
        .Query<ContentItem, MeasurementDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync()
        .Result;
      if (contentItem is null)
      {
        continue;
      }

      handler.Handle(@event.DeviceId, contentItem, @event.Payload);
    }
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
      var handler = services
        .GetServices<IUpdateHandler>()
        .FirstOrDefault(handler => handler.Id == @event.HandlerId);
      if (handler is null)
      {
        continue;
      }

      var contentItem = await session
        .Query<ContentItem, MeasurementDeviceIndex>()
        .Where(index => index.DeviceId == @event.DeviceId)
        .FirstOrDefaultAsync();
      if (contentItem is null)
      {
        continue;
      }

      await handler.HandleAsync(@event.DeviceId, contentItem, @event.Payload);
      if (cancellationToken.IsCancellationRequested)
      {
        return;
      }
    }
  }
}
