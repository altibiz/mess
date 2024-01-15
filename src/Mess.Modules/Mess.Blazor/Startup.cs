using Mess.Blazor.Abstractions.Components;
using Mess.Blazor.Abstractions.Localization;
using Mess.Blazor.Abstractions.ShapeTemplateStrategy;
using Mess.Blazor.Components;
using Mess.Blazor.Controllers;
using Mess.Blazor.ShapeTemplateStrategy;
using Mess.Cms.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Modules;
using YesSql;

namespace Mess.Blazor;

// FIXME: app redirection not working

public class Startup : OrchardCore.Modules.StartupBase
{
  private readonly IHostEnvironment _hostEnvironment;

  public Startup(IHostEnvironment hostEnvironment)
  {
    _hostEnvironment = hostEnvironment;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<ResourceMiddleware>();

    services.AddServerSideBlazor().AddCircuitOptions(options =>
    {
      if (_hostEnvironment.IsDevelopment())
      {
        options.DetailedErrors = true;
      }
    }).AddHubOptions(options =>
    {
      if (_hostEnvironment.IsDevelopment())
      {
        options.EnableDetailedErrors = true;
      }
    });

    services.AddScoped<CircuitAccessor>();
    services.AddScoped<ICircuitAccessor>(services => services
      .GetRequiredService<CircuitAccessor>());

    services.AddScoped<CircuitHandler, ComponentCaptureCircuitHandler>();
    services
      .AddSingleton<IComponentCaptureStore, ComponentCaptureStore>();

    services
      .AddScoped<IShapeComponentHarvester, BasicShapeComponentHarvester>();
    services
      .AddScoped<IShapeTemplateComponentEngine,
        BlazorShapeTemplateComponentEngine>();
    services.AddScoped<IShapeTableProvider, ShapeComponentBindingStrategy>();
    services.AddScoped<ComponentHelper>();

    services.AddMudServices();
    services.AddScoped<IComponentLocalizer, ComponentLocalizer>();

    // services.AddScoped<AsyncLocalSessionManager>();
    // services.AddTransient(services =>
    //   services.GetRequiredService<AsyncLocalSessionManager>().Session
    // );

    services.AddScoped<AppQueryExecutor>();
    services.AddScoped<IAppQueryExecutor>(services => services
      .GetRequiredService<AppQueryExecutor>());
    services.AddScoped<IComponentQueryDispatcher>(services => services
      .GetRequiredService<AppQueryExecutor>());
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

    app.UseStaticFiles(new StaticFileOptions
    {
      FileProvider =
        new ManifestEmbeddedFileProvider(typeof(WebAssemblyHostBuilder)
          .Assembly)
    });

    // app.UseEndpoints(endpoints =>
    // {
    //   endpoints.Redirect("/", "/app");
    // });

    app.UseMiddleware<ResourceMiddleware>();

    routes.MapAreaControllerRoute<AppController>(
      nameof(AppController.Index),
      "/App/{**catchall}"
    );

    routes.MapBlazorHub();
  }
}
