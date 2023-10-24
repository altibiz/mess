using Marten;
using Mess.Event.Abstractions.Session;

namespace Mess.Event.Abstractions.Extensions;

public static class EventStoreQueryServiceProviederExtensions
{
  public static async Task<T> WithEventStoreQueryAsync<T>(
    this IServiceProvider services,
    Func<IQuerySession, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var session = scope.ServiceProvider.GetRequiredService<IEventStoreQuery>();
    return await todo(session.Value);
  }

  public static T WithEventStoreQuery<T>(
    this IServiceProvider services,
    Func<IQuerySession, T> todo
  )
  {
    using var scope = services.CreateScope();
    var session = scope.ServiceProvider.GetRequiredService<IEventStoreQuery>();
    return todo(session.Value);
  }
}
