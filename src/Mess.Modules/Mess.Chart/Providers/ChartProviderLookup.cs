using Microsoft.Extensions.DependencyInjection;
using Mess.Chart.Abstractions.Providers;

namespace Mess.Chart.Providers;

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

  public bool Exists(string id)
  {
    return _providers.ContainsKey(id);
  }

  public IReadOnlyList<string> Ids => _providers.Keys.ToList();

  public ChartProviderLookup(IServiceProvider services)
  {
    _providers = services
      .GetServices<IChartProvider>()
      .ToDictionary(provider => provider.Id);
  }

  private readonly IDictionary<string, IChartProvider> _providers;
}
