using Mess.Population.Abstractions;
using OrchardCore.Modules;

namespace Mess.Population;

public class PopulationTenantEvents : ModularTenantEvents
{
  public override async Task ActivatedAsync()
  {
    foreach (var population in _populations)
    {
      await population.PopulateAsync();
    }
  }

  public PopulationTenantEvents(IEnumerable<IPopulation> populations)
  {
    _populations = populations.ToList();
  }

  private readonly List<IPopulation> _populations;
}
