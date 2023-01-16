using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Mess.EventStore.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.EventStore.Services;

public class Projection : IProjection
{
  public IServiceProvider? Services { get; set; }

  public void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  )
  {
    if (Services is null)
    {
      throw new InvalidOperationException("No services");
    }

    using var scope = Services.CreateScope();

    var events = new Events(streams);

    var dispatchers =
      scope.ServiceProvider.GetServices<IProjectionDispatcher>();

    foreach (var dispatcher in dispatchers)
    {
      dispatcher.Apply(Services, events);
    }
  }

  public async Task ApplyAsync(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellationToken
  )
  {
    if (Services is null)
    {
      throw new InvalidOperationException("No services");
    }

    await using var scope = Services.CreateAsyncScope();

    var events = new Events(streams);

    var dispatchers =
      scope.ServiceProvider.GetServices<IProjectionDispatcher>();

    foreach (var dispatcher in dispatchers)
    {
      await dispatcher.ApplyAsync(Services, events, cancellationToken);
    }
  }
}
