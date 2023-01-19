using Mess.Chart.Abstractions.Providers;

namespace Mess.Chart.Abstractions.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static void RegisterChartProvider<T>(this IServiceCollection services)
    where T : class, IChartDataProvider
  {
    services.AddScoped<IChartDataProvider, T>();
  }
}
