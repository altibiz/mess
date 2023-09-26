namespace Mess.Event.Abstractions.Events;

public interface IProjectionApplicator
{
  public void Apply(IServiceProvider services, IEvents events);

  public Task ApplyAsync(
    IServiceProvider services,
    IEvents events,
    CancellationToken cancellationToken
  );
}
