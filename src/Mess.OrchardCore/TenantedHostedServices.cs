using OrchardCore.Modules;

namespace Mess.OrchardCore;

public class HostedServiceModularTenantEvents<T> : ModularTenantEvents
  where T : IHostedService
{
  public override async Task ActivatedAsync()
  {
    await _hostedService.StartAsync(default);
  }

  public override async Task TerminatingAsync()
  {
    await _hostedService.StopAsync(default);
  }

  public HostedServiceModularTenantEvents(T hostedService)
  {
    _hostedService = hostedService;
  }

  private readonly T _hostedService;
}