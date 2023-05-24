using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Abstractions.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddProjectionDispatcher<T>(
    this IServiceCollection services
  )
    where T : class, IProjectionDispatcher
  {
    services.AddScoped<IProjectionDispatcher, T>();
  }
}
