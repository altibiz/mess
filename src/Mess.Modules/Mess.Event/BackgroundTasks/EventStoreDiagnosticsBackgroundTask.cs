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

    var allProgress = await store.Advanced.AllProjectionProgress();
    foreach (var state in allProgress)
    {
      logger.LogInformation($"{state.ShardName} is at {state.Sequence}");
    }

    var stats = await store.Advanced.FetchEventStoreStatistics();
    logger.LogInformation(
      $"The event store highest sequence is {stats.EventSequenceNumber}",
      logger
    );

    var daemonHighWaterMark = await store.Advanced.ProjectionProgressFor(
      new ShardName(ShardState.HighWaterMark)
    );
    logger.LogInformation(
      $"The daemon high water sequence mark is {daemonHighWaterMark}"
    );
  }
}
