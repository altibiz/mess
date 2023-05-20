using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Abstractions.Client;

public interface IEventStoreClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void RecordEvents<T>(params IEvent[] events)
    where T : class;

  public Task RecordEventsAsync<T>(params IEvent[] events)
    where T : class;

  public void RecordEvents(Type aggregateType, params IEvent[] events);

  public Task RecordEventsAsync(Type aggregateType, params IEvent[] events);

  public IReadOnlyList<(Type AggregateType, IReadOnlyList<IEvent>)> Export(
    CancellationToken? cancellationToken = null
  );

  public Task<
    IReadOnlyList<(Type AggregateType, IReadOnlyList<IEvent>)>
  > ExportAsync(CancellationToken? cancellationToken = null);
}
