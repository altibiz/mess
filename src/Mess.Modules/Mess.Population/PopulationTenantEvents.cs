using Mess.Population.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Models;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Modules;
using YesSql;
using Environment = OrchardCore.Environment;

namespace Mess.Population;

// TODO: this isn't idempotent and it should work like migrations

public class PopulationTenantEvents : ModularTenantEvents
{
  public override async Task ActivatingAsync()
  {
    ShellScope.AddDeferredTask(async scope =>
    {
      var hostEnvironment =
        scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
      if (!hostEnvironment.IsDevelopment()) return;

      var shellSettings =
        scope.ServiceProvider.GetRequiredService<ShellSettings>();
      if (
        shellSettings.State != TenantState.Initializing
      )
        return;

      var populations = scope.ServiceProvider.GetServices<IPopulation>();
      foreach (var population in populations) await population.PopulateAsync();

      var session = scope.ServiceProvider.GetRequiredService<ISession>();
      await session.SaveChangesAsync();
    });
  }
}
