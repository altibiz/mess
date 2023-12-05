using Mess.Cms.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace Mess.Blazor;

public class Startup : OrchardCore.Modules.StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddServerSideBlazor();

    services.AddScoped<ScriptMiddleware>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    app.UseStaticFiles(new StaticFileOptions
    {
      FileProvider = new ManifestEmbeddedFileProvider(typeof(IServerSideBlazorBuilder).Assembly),
    });

    app.UseMiddleware<ScriptMiddleware>();

    routes.MapBlazorHub();
  }
}
