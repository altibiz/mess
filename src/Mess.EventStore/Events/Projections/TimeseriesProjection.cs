using Marten;
using Marten.Events;
using Marten.Events.Projections;

namespace Mess.EventStore.Events.Projections;

public class TimeseriesProjection : IProjection
{
  public void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  ) { }

  public Task ApplyAsync(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellation
  )
  {
    return Task.CompletedTask;
  }
}
