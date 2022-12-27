using Marten;
using Marten.Events.Daemon;

namespace Mess.EventStore.Extensions.Marten;

public static class IDocumentStoreExtensions
{
  // https://martendb.io/events/projections/async-daemon.html
  public static async Task ShowDaemonDiagnostics(this IDocumentStore store)
  {
    // This will tell you the current progress of each known projection shard
    // according to the latest recorded mark in the database
    var allProgress = await store.Advanced.AllProjectionProgress();
    foreach (var state in allProgress)
    {
      Console.WriteLine($"{state.ShardName} is at {state.Sequence}");
    }

    // This will allow you to retrieve some basic statistics about the event
    // store
    var stats = await store.Advanced.FetchEventStoreStatistics();
    Console.WriteLine(
      $"The event store highest sequence is {stats.EventSequenceNumber}"
    );

    // This will let you fetch the current shard state of a single projection
    // shard, but in this case we're looking for the daemon high water mark
    var daemonHighWaterMark = await store.Advanced.ProjectionProgressFor(
      new ShardName(ShardState.HighWaterMark)
    );
    Console.WriteLine(
      $"The daemon high water sequence mark is {daemonHighWaterMark}"
    );
  }
}
