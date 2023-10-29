namespace Mess.Event.Abstractions.Services;

public interface IEvents
{
  public IReadOnlyList<T> OfType<T>()
    where T : IEvent;
}
