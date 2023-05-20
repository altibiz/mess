using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Mess.EventStore.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.EventStore.Services;

public record class Projection(IServiceProvider Services) : IProjection
{
  public void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  )
  {
    using var scope = Services.CreateScope();

    var events = new Events(streams);

    var dispatchers =
      scope.ServiceProvider.GetServices<IProjectionDispatcher>();

    foreach (var dispatcher in dispatchers)
    {
      dispatcher.Apply(scope.ServiceProvider, events);
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

    var dispatchers =
      scope.ServiceProvider.GetServices<IProjectionDispatcher>();

    foreach (var dispatcher in dispatchers)
    {
      await dispatcher.ApplyAsync(
        scope.ServiceProvider,
        events,
        cancellationToken
      );
    }
  }
}
