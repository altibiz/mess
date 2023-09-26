using Marten;
using Mess.Event.Abstractions.Session;

namespace Mess.Event.Abstractions.Extensions;

public static class EventStoreSessionServiceProviederExtensions
{
  public static async Task<T> WithEventStoreSessionAsync<T>(
    this IServiceProvider services,
    Func<IDocumentSession, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var session =
      scope.ServiceProvider.GetRequiredService<IEventStoreSession>();
    var result = await todo(session.Value);
    await session.Value.SaveChangesAsync();
    return result;
  }

  public static T WithEventStoreSession<T>(
    this IServiceProvider services,
    Func<IDocumentSession, T> todo
  )
  {
    using var scope = services.CreateScope();
    var session =
      scope.ServiceProvider.GetRequiredService<IEventStoreSession>();
    var result = todo(session.Value);
    session.Value.SaveChanges();
    return result;
  }
}
