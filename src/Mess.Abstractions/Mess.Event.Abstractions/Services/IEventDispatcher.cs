namespace Mess.Event.Abstractions.Events;

public interface IEventDispatcher
{
  public void Dispatch(IServiceProvider services, IEvents events);

  public Task DispatchAsync(
    IServiceProvider services,
    IEvents events,
    CancellationToken cancellationToken
  );
}
