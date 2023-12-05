using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using OrchardCore.Modules;

namespace Mess.Blazor;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddServerSideBlazor();

    services.AddScoped<ResourceMiddleware>();

    services.AddMudServices();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    app.UseStaticFiles(new StaticFileOptions
    {
      FileProvider =
        new ManifestEmbeddedFileProvider(typeof(IServerSideBlazorBuilder)
          .Assembly)
    });

    app.UseMiddleware<ResourceMiddleware>();

    routes.MapBlazorHub();
  }
}
