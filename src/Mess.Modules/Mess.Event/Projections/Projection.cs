using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Mess.Event.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mess.Event.Projections;

public record class Projection(IServiceProvider Services) : IProjection
{
  public void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  )
{
  using var scope = Services.CreateScope();
  var dispatchers = scope.ServiceProvider.GetServices<IEventDispatcher>();
  var logger = scope.ServiceProvider.GetRequiredService<
    ILogger<Projection>
  >();

  var events = new Events(streams);

  foreach (var dispatcher in dispatchers)
  {
    try
    {
      dispatcher.Dispatch(scope.ServiceProvider, events);
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "Error while dispatching events of {}",
        dispatcher.GetType().Name
      );
    }
  }
}

public async Task ApplyAsync(
  IDocumentOperations operations,
  IReadOnlyList<StreamAction> streams,
  CancellationToken cancellation
)
{
  await using var scope = Services.CreateAsyncScope();
  var dispatchers = scope.ServiceProvider.GetServices<IEventDispatcher>();
  var logger = scope.ServiceProvider.GetRequiredService<
    ILogger<Projection>
  >();

  var events = new Events(streams);

  foreach (var dispatcher in dispatchers)
  {
    try
    {
      await dispatcher.DispatchAsync(
        scope.ServiceProvider,
        events,
        cancellation
      );
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "Error while dispatching events of {}",
        dispatcher.GetType().Name
      );
    }
  }
}
}
