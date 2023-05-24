using Marten.Events;
using Mess.EventStore.Abstractions.Events;
using IEvent = Mess.EventStore.Abstractions.Events.IEvent;

namespace Mess.EventStore.Services;

public class Events : IEvents
{
  private readonly IReadOnlyList<StreamAction> _streams;

  public Events(IReadOnlyList<StreamAction> streams)
  {
    _streams = streams;
  }

  public IReadOnlyList<T> OfType<T>()
    where T : IEvent =>
    _streams
      .SelectMany(stream => stream.Events)
      .Where(@event => @event.EventType == typeof(T))
      .OrderBy(@event => @event.Sequence)
      .Select(@event => @event.Data)
      .Cast<T>()
      .Where(@event => @event is not null)
      .ToList();
}
