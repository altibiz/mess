using Mess.Timeseries.Abstractions.Context;

namespace Mess.Timeseries.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddTimeseriesDbContext<T>(this IServiceCollection services)
    where T : TimeseriesDbContext
  {
    services.AddDbContext<T>();
  }

  public static void AddTimeseriesClient<T, C, Q>(
    this IServiceCollection services
  )
    where T : class, C, Q
    where C : class, Q
    where Q : class
  {
    services.AddSingleton<C, T>();
    services.AddSingleton<Q>(services => services.GetRequiredService<C>());
  }
}
