using Mess.Relational.Abstractions.Context;

namespace Mess.Relational.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddTimeseriesDbContext<T>(this IServiceCollection services)
    where T : RelationalDbContext
  {
    services.AddDbContext<T>();
  }

  public static void AddRelationalClient<T, C, Q>(
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
