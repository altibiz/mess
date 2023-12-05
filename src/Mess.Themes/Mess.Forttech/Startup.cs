using Mess.Cms.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Forttech;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddResources<Resources>();
  }
}
