using Mess.Blazor.Abstractions.Components;
using Mess.Blazor.Abstractions.Localization;
using Mess.Blazor.Abstractions.ShapeTemplateStrategy;
using Mess.Blazor.Components;
using Mess.Blazor.Controllers;
using Mess.Blazor.ShapeTemplateStrategy;
using Mess.Cms.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Modules;

namespace Mess.Blazor;

public class Startup : StartupBase
{
  private readonly IHostEnvironment _hostEnvironment;

  public Startup(IHostEnvironment hostEnvironment)
  {
    _hostEnvironment = hostEnvironment;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddServerSideBlazor().AddCircuitOptions(options =>
    {
      if (_hostEnvironment.IsDevelopment())
      {
        options.DetailedErrors = true;
      }
    });

    services.AddScoped<ResourceMiddleware>();

    services
      .AddScoped<IShapeComponentHarvester, BasicShapeComponentHarvester>();
    services
      .AddScoped<IShapeTemplateComponentEngine,
        BlazorShapeTemplateComponentEngine>();
    services.AddScoped<IShapeTableProvider, ShapeComponentBindingStrategy>();

    services.AddMudServices();

    services
      .AddSingleton<IShapeComponentModelStore, ShapeComponentModelStore>();

    services.AddScoped<CircuitHandler, ShapeComponentCircuitHandler>();

    services.AddScoped<ShapeComponentCircuitAccessor>();
    services.AddScoped<IShapeComponentCircuitAccessor>(
      services => services
        .GetRequiredService<ShapeComponentCircuitAccessor>());

    services.AddScoped<IComponentLocalizer, ComponentLocalizer>();

    services.AddScoped<ComponentHelper>();
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

    routes.MapAreaControllerRoute<AppController>(
      nameof(AppController.Index),
      "/App"
    );

    routes.MapBlazorHub();
  }
}
