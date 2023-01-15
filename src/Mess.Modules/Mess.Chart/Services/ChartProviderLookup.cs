using Microsoft.Extensions.DependencyInjection;
using Mess.Chart.Abstractions.Providers;

namespace Mess.Chart.Services;

public class ChartProviderLookup : IChartProviderLookup
{
  public IChartProvider? Get(string id)
  {
    if (_providers.TryGetValue(id, out var provider))
    {
      return provider;
    }

    return null;
  }

  public ChartProviderLookup(IServiceProvider services)
  {
    _providers = services
      .GetServices<IChartProvider>()
      .ToDictionary(provider => provider.Id);
  }

  private readonly IDictionary<string, IChartProvider> _providers;
}
