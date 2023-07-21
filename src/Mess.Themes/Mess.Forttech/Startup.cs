using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.OrchardCore.Extensions.Microsoft;

namespace Mess.Forttech;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddResources<Resources>();
  }
}
