using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using OrchardCore.Modules;
using MudBlazor.Services;
using Mess.Blazor.Abstractions;
using Mess.Blazor.Abstractions.ShapeTemplateStrategy;
using Mess.Blazor.ShapeTemplateStrategy;

namespace Mess.Blazor;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddServerSideBlazor();

    services.AddScoped<ResourceMiddleware>();

    services.AddScoped<IShapeComponentHarvester, BasicShapeComponentHarvester>();
    services.AddScoped<IShapeTemplateComponentEngine, BlazorShapeTemplateComponentEngine>();
    services.AddScoped<IShapeTableProvider, ShapeComponentBindingStrategy>();

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
