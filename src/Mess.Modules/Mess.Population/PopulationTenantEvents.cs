using Mess.Population.Abstractions;
using Mess.System.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Population;

public class PopulationTenantEvents : IModularTenantEvents
{
  public async Task ActivatedAsync()
  {
    // TODO: scope each service?
    await _serviceProvider.AwaitScopeAsync(async serviceProvider =>
    {
      var populations = serviceProvider.GetServices<IPopulation>();

      foreach (var population in populations)
      {
        await population.PopulateAsync();
      }
    });
  }

  public Task ActivatingAsync()
  {
    return Task.CompletedTask;
  }

  public Task TerminatedAsync()
  {
    return Task.CompletedTask;
  }

  public Task TerminatingAsync()
  {
    return Task.CompletedTask;
  }

  public PopulationTenantEvents(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
