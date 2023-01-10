using Microsoft.Extensions.Logging;
using Marten.Schema;
using Marten;
using Mess.EventStore.Events;

namespace Mess.EventStore.Client;

public class EventStoreConnectionCheckDocumentType
{
  [Identity]
  public string? Id = null;
}

public class EventStoreClient : IEventStoreClient
{
  public Task<bool> CheckConnectionAsync() =>
    Services.WithEventStoreQueryAsync(async query =>
    {
      var result = await query
        .Query<EventStoreConnectionCheckDocumentType>()
        .FirstOrDefaultAsync<EventStoreConnectionCheckDocumentType>(
          CancellationToken.None
        );
      var connected = true;

      LogConenction(connected);

      return connected;
    });

  public bool CheckConnection() =>
    Services.WithEventStoreQuery(query =>
    {
      var result = query
        .Query<EventStoreConnectionCheckDocumentType>()
        .FirstOrDefault();
      var connected = true;

      LogConenction(connected);

      return connected;
    });

  public void RecordEvents<T>(params object[] events) where T : class =>
    Services.WithEventStoreSession(session =>
    {
      session.Events.StartStream<T>(Guid.NewGuid(), events);
      session.SaveChanges();

      return true;
    });

  public Task RecordEventsAsync<T>(params object[] events) where T : class =>
    Services.WithEventStoreSessionAsync(
      async (session) =>
      {
        session.Events.StartStream<T>(Guid.NewGuid(), events);
        await session.SaveChangesAsync();

        return true;
      }
    );

  public T? LastEvent<T>() where T : Event =>
    Services.WithEventStoreQuery(
      query =>
        query
          .Query<T>()
          .OrderByDescending(@event => @event.timestamp)
          .FirstOrDefault()
    );

  public Task<T?> LastEventAsync<T>() where T : Event =>
    Services.WithEventStoreQueryAsync(
      async (query) =>
        await query
          .Query<T>()
          .OrderByDescending(@event => @event.timestamp)
          .FirstOrDefaultAsync()
    );

  private void LogConenction(bool connected)
  {
    if (connected)
    {
      Logger.LogInformation("Connected to EventStore server");
    }
    else
    {
      Logger.LogInformation("Failed connecting to EventStore server");
    }
  }

  public EventStoreClient(
    IServiceProvider services,
    ILogger<EventStoreClient> logger
  )
  {
    Services = services;
    Logger = logger;
  }

  private IServiceProvider Services { get; }
  private ILogger Logger { get; }
}