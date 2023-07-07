using Microsoft.Extensions.Logging;
using Marten.Schema;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Abstractions.Events;
using Mess.EventStore.Abstractions.Extensions.Microsoft;

namespace Mess.EventStore.Client;

public class EventStoreConnectionCheckDocumentType
{
  [Identity]
  public string? Id = null;
}

public class EventStoreClient : IEventStoreClient
{
  public void RecordEvents<T>(params IEvent[] events)
    where T : class =>
    _services.WithEventStoreSession(session =>
    {
      session.Events.StartStream<T>(Guid.NewGuid(), events);
      session.SaveChanges();

      return true;
    });

  public Task RecordEventsAsync<T>(params IEvent[] events)
    where T : class =>
    _services.WithEventStoreSessionAsync(
      async (session) =>
      {
        session.Events.StartStream<T>(Guid.NewGuid(), events);
        await session.SaveChangesAsync();

        return true;
      }
    );

  public void RecordEvents(Type aggregateType, params IEvent[] events) =>
    _services.WithEventStoreSession(session =>
    {
      session.Events.StartStream(aggregateType, Guid.NewGuid(), events);
      session.SaveChanges();

      return true;
    });

  public Task RecordEventsAsync(Type aggregateType, params IEvent[] events) =>
    _services.WithEventStoreSessionAsync(
      async (session) =>
      {
        session.Events.StartStream(aggregateType, Guid.NewGuid(), events);
        await session.SaveChangesAsync();

        return true;
      }
    );

  public IReadOnlyList<(Type AggregateType, IReadOnlyList<IEvent>)> Export(
    CancellationToken? cancellationToken = null
  ) =>
    throw new NotImplementedException(
      "Marten has no official way of exporting all events"
    );

  public Task<
    IReadOnlyList<(Type AggregateType, IReadOnlyList<IEvent>)>
  > ExportAsync(CancellationToken? cancellationToken = null) =>
    throw new NotImplementedException(
      "Marten has no official way of exporting all events"
    );

  public EventStoreClient(
    IServiceProvider services,
    ILogger<EventStoreClient> logger
  )
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
