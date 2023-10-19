using Mess.Population.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Modules;
using Environment = OrchardCore.Environment;

namespace Mess.Population;


// TODO: this isn't idempotent and it should work like migrations

public class PopulationTenantEvents : ModularTenantEvents
{
  public override async Task ActivatingAsync()
  {
    if (!_hostEnvironment.IsDevelopment())
    {
      return;
    }

    if (
      _shellSettings.State != Environment.Shell.Models.TenantState.Initializing
    )
    {
      return;
    }

    ShellScope.AddDeferredTask(async scope =>
    {
      var populations = scope.ServiceProvider.GetServices<IPopulation>();

      foreach (var population in populations)
      {
        await population.PopulateAsync();
      }
    });
  }

  public PopulationTenantEvents(
    ShellSettings shellSettings,
    IHostEnvironment hostEnvironment
  )
  {
    _shellSettings = shellSettings;
    _hostEnvironment = hostEnvironment;
  }

  private readonly ShellSettings _shellSettings;

  private readonly IHostEnvironment _hostEnvironment;
}
