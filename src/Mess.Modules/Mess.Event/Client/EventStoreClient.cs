using Mess.Event.Abstractions.Client;
using Mess.Event.Abstractions.Extensions;
using Mess.Event.Abstractions.Services;

namespace Mess.Event.Client;

public class EventStoreClient : IEventStoreClient
{
  private readonly IServiceProvider _services;

  public EventStoreClient(
    IServiceProvider services
  )
  {
    _services = services;
  }

  public void RecordEvents<T>(params IEvent[] events)
    where T : class
  {
    _services.WithEventStoreSession(session =>
    {
      session.Events.StartStream<T>(Guid.NewGuid(), events);
      session.SaveChanges();

      return true;
    });
  }

  public Task RecordEventsAsync<T>(params IEvent[] events)
    where T : class
  {
    return _services.WithEventStoreSessionAsync(
      async session =>
      {
        session.Events.StartStream<T>(Guid.NewGuid(), events);
        await session.SaveChangesAsync();

        return true;
      }
    );
  }

  public void RecordEvents(Type aggregateType, params IEvent[] events)
  {
    _services.WithEventStoreSession(session =>
    {
      session.Events.StartStream(aggregateType, Guid.NewGuid(), events);
      session.SaveChanges();

      return true;
    });
  }

  public Task RecordEventsAsync(Type aggregateType, params IEvent[] events)
  {
    return _services.WithEventStoreSessionAsync(
      async session =>
      {
        session.Events.StartStream(aggregateType, Guid.NewGuid(), events);
        await session.SaveChangesAsync();

        return true;
      }
    );
  }

  public IReadOnlyList<(Type AggregateType, IReadOnlyList<IEvent>)> Export(
    CancellationToken? cancellationToken = null
  )
  {
    throw new NotImplementedException(
      "Marten has no official way of exporting all events"
    );
  }

  public Task<
    IReadOnlyList<(Type AggregateType, IReadOnlyList<IEvent>)>
  > ExportAsync(CancellationToken? cancellationToken = null)
  {
    throw new NotImplementedException(
      "Marten has no official way of exporting all events"
    );
  }
}
