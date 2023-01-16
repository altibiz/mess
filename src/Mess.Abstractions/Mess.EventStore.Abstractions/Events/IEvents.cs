namespace Mess.EventStore.Abstractions.Events;

public interface IEvents
{
  public IReadOnlyList<T> OfType<T>() where T : IEvent;
}
