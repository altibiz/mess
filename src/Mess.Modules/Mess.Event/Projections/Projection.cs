using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Mess.Event.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.Event.Projections;

public record class Projection(IServiceProvider Services) : IProjection
{
  public void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  )
  {
    using var scope = Services.CreateScope();

    var events = new Events(streams);

    var applicators = scope.ServiceProvider.GetServices<IEventDispatcher>();

    foreach (var applicator in applicators)
    {
      applicator.Dispatch(scope.ServiceProvider, events);
    }
  }

  public async Task ApplyAsync(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellationToken
  )
  {
    await using var scope = Services.CreateAsyncScope();

    var events = new Events(streams);

    var applicators = scope.ServiceProvider.GetServices<IEventDispatcher>();

    foreach (var applicator in applicators)
    {
      await applicator.DispatchAsync(
        scope.ServiceProvider,
        events,
        cancellationToken
      );
    }
  }
}
