using Mess.Cms.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Helb;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddResources<Resources>();
  }
}
