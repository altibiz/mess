using Marten;
using Marten.Events.Daemon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;

namespace Mess.Event.BackgroundTasks;

[BackgroundTask(Schedule = "*/5 * * * *")]
public class EventStoreDiagnosticsBackgroundTask : IBackgroundTask
{
  public async Task DoWorkAsync(
    IServiceProvider serviceProvider,
    CancellationToken cancellationToken
  )
  {
    var store = serviceProvider.GetRequiredService<IDocumentStore>();
    var logger = serviceProvider.GetRequiredService<
      ILogger<EventStoreDiagnosticsBackgroundTask>
    >();

    var allProgress =
      await store.Advanced.AllProjectionProgress(token: cancellationToken);
    foreach (var state in allProgress)
      logger.LogInformation("{} is at {}", state.ShardName, state.Sequence);

    var stats =
      await store.Advanced.FetchEventStoreStatistics(token: cancellationToken);
    logger.LogInformation(
      "The event store highest sequence is {}",
      stats.EventSequenceNumber
    );

    var daemonHighWaterMark = await store.Advanced.ProjectionProgressFor(
      new ShardName(ShardState.HighWaterMark),
      token: cancellationToken
    );
    logger.LogInformation(
      "The daemon high water sequence mark is {}",
      daemonHighWaterMark
    );
  }
}
