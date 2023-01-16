namespace Mess.EventStore.Abstractions.Events;

public interface IProjectionDispatcher
{
  public void Apply(IServiceProvider services, IEvents events);

  public Task ApplyAsync(
    IServiceProvider services,
    IEvents events,
    CancellationToken cancellationToken
  );
}
