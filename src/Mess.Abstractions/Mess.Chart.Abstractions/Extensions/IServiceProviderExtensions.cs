using Mess.Chart.Abstractions.Services;

namespace Mess.Chart.Abstractions.Extensions;

public static class IServiceProviderExtensions
{
  public static void AddChartFactory<T>(this IServiceCollection services)
    where T : class, IChartFactory
  {
    services.AddScoped<IChartFactory, T>();
  }
}
