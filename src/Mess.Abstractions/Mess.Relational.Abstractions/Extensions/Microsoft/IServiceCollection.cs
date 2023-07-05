using Mess.Relational.Abstractions.Context;

namespace Mess.Relational.Abstractions.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddTimeseriesDbContext<T>(this IServiceCollection services)
    where T : RelationalDbContext
  {
    services.AddDbContext<T>();
  }
}
