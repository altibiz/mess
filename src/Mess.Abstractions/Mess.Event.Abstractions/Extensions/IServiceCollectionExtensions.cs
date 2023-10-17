using Mess.Event.Abstractions.Events;

namespace Mess.Event.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddEventDispatcher<T>(this IServiceCollection services)
    where T : class, IEventDispatcher
  {
    services.AddScoped<IEventDispatcher, T>();
  }
}
