using Marten;
using Marten.Events;
using Marten.Events.Projections;

namespace Mess.EventStore.Events.Projections;

public abstract class Projection : IProjection
{
  public abstract void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  );

  public abstract Task ApplyAsync(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellation
  );
}
