using Marten.Events;

namespace Mess.EventStore.Abstractions.Events;

public interface IProjectionDispatcher
{
  public void Apply(
    IServiceProvider services,
    IReadOnlyList<StreamAction> streams
  );

  public Task ApplyAsync(
    IServiceProvider services,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellationToken
  );
}
