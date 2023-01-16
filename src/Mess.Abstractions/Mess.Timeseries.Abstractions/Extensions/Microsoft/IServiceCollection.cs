using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Abstractions.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void RegisterTimeseriesDbContext<T>(
    this IServiceCollection services
  ) where T : TimeseriesDbContext
  {
    services.AddDbContext<T>();
  }
}
