using Mess.Timeseries.Abstractions.Context;

namespace Mess.Timeseries.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddTimeseriesDbContext<T>(this IServiceCollection services)
    where T : TimeseriesDbContext
  {
    services.AddDbContext<T>();
  }

  public static void AddTimeseriesClient<TImplementation, TClient, TQuery>(
    this IServiceCollection services
  )
    where TImplementation : class, TClient, TQuery
    where TClient : class, TQuery
    where TQuery : class
  {
    services.AddSingleton<TClient, TImplementation>();
    services.AddSingleton<TQuery>(
      services => services.GetRequiredService<TClient>()
    );
  }
}
