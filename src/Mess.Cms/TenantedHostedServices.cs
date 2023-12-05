using OrchardCore.Modules;

namespace Mess.Cms;

public class HostedServiceModularTenantEvents<T> : ModularTenantEvents
  where T : IHostedService
{
  private readonly T _hostedService;

  public HostedServiceModularTenantEvents(T hostedService)
  {
    _hostedService = hostedService;
  }

  public override async Task ActivatedAsync()
  {
    await _hostedService.StartAsync(default);
  }

  public override async Task TerminatingAsync()
  {
    await _hostedService.StopAsync(default);
  }
}
