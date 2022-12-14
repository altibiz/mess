using OrchardCore.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Mess.EventStore
{
  public class Startup : StartupBase
  {
    public override void ConfigureServices(IServiceCollection services) { }

    public override void Configure(
      IApplicationBuilder builder,
      IEndpointRouteBuilder routes,
      IServiceProvider serviceProvider
    )
    {
      routes.MapAreaControllerRoute(
        name: "Home",
        areaName: "Mess.EventStore",
        pattern: "Home/Index",
        defaults: new { controller = "Home", action = "Index" }
      );
    }
  }
}
