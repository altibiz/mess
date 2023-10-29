using Mess.Relational.Abstractions.Context;

namespace Mess.Relational.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddTimeseriesDbContext<T>(this IServiceCollection services)
    where T : RelationalDbContext
  {
    services.AddDbContext<T>();
  }

  public static void AddRelationalClient<TImplementation, TClient, TQuery>(
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
