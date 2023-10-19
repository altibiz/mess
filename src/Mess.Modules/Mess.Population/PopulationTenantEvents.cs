using OrchardCore.Modules;

namespace Mess.Population;

public class PopulationTenantEvents : IModularTenantEvents
{
  public async Task ActivatedAsync() { }

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
}
