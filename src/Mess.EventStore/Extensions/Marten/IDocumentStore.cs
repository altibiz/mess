using Marten;
using Marten.Events.Daemon;
using Microsoft.Extensions.Logging;

namespace Mess.EventStore.Extensions.Marten;

// NOTE: https://martendb.io/events/projections/async-daemon.html

public static class IDocumentStoreExtensions
{
  public static async Task ShowDaemonDiagnosticsAsync(
    this IDocumentStore store,
    ILogger? logger = null
  )
  {
    // This will tell you the current progress of each known projection shard
    // according to the latest recorded mark in the database
    var allProgress = await store.Advanced.AllProjectionProgress();
    foreach (var state in allProgress)
    {
      Log($"{state.ShardName} is at {state.Sequence}", logger);
    }

    // This will allow you to retrieve some basic statistics about the event
    // store
    var stats = await store.Advanced.FetchEventStoreStatistics();
    Log(
      $"The event store highest sequence is {stats.EventSequenceNumber}",
      logger
    );

    // This will let you fetch the current shard state of a single projection
    // shard, but in this case we're looking for the daemon high water mark
    var daemonHighWaterMark = await store.Advanced.ProjectionProgressFor(
      new ShardName(ShardState.HighWaterMark)
    );
    Log(
      $"The daemon high water sequence mark is {daemonHighWaterMark}",
      logger
    );
  }

  public static void ShowDaemonDiagnostics(
    this IDocumentStore store,
    ILogger? logger = null
  )
  {
    var allProgress = store.Advanced.AllProjectionProgress().Result;
    foreach (var state in allProgress)
    {
      Log($"{state.ShardName} is at {state.Sequence}", logger);
    }

    var stats = store.Advanced.FetchEventStoreStatistics().Result;
    Log(
      $"The event store highest sequence is {stats.EventSequenceNumber}",
      logger
    );

    var daemonHighWaterMark = store.Advanced
      .ProjectionProgressFor(new ShardName(ShardState.HighWaterMark))
      .Result;
    Log(
      $"The daemon high water sequence mark is {daemonHighWaterMark}",
      logger
    );
  }

  private static void Log(string message, ILogger? logger = null)
  {
    if (logger is null)
    {
      Console.WriteLine(message);
    }
    else
    {
      logger.LogInformation(message);
    }
  }
}
