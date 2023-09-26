using Mess.Event.Abstractions.Events;

namespace Mess.Event.Abstractions.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddProjectionDispatcher<T>(
    this IServiceCollection services
  )
    where T : class, IProjectionApplicator
  {
    services.AddScoped<IProjectionApplicator, T>();
  }
}
