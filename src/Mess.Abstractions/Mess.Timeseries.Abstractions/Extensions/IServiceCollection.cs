using Mess.Timeseries.Abstractions.Context;

namespace Mess.Timeseries.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddTimeseriesDbContext<T>(this IServiceCollection services)
    where T : TimeseriesDbContext
  {
    services.AddDbContext<T>();
  }
}
