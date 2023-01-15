using Fluid;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Controllers;
using Mess.Chart.Drivers;
using Mess.Chart.Fields;
using Mess.Chart.Handlers;
using Mess.Chart.Indexing;
using Mess.Chart.Models;
using Mess.Chart.Services;
using Mess.Chart.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Indexing;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ResourceManagement;

namespace Mess.Chart;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();

    services.Configure<TemplateOptions>(o =>
    {
      o.MemberAccessStrategy.Register<ChartField>();
      o.MemberAccessStrategy.Register<DisplayChartFieldViewModel>();
    });

    services
      .AddContentField<ChartField>()
      .UseDisplayDriver<ChartFieldDisplayDriver>();
    services.AddScoped<
      IContentPartFieldDefinitionDisplayDriver,
      ChartFieldSettingsDriver
    >();
    services.AddScoped<IContentFieldIndexHandler, ChartFieldIndexHandler>();

    services
      .AddContentPart<ChartPart>()
      .UseDisplayDriver<ChartPartDisplayDriver>()
      .AddHandler<ChartPartHandler>();
    services.AddScoped<
      IContentTypePartDefinitionDisplayDriver,
      ChartPartSettingsDisplayDriver
    >();
    services.AddScoped<IContentPartIndexHandler, ChartPartIndexHandler>();

    services.AddScoped<IChartProviderLookup, ChartProviderLookup>();
  }

  public override void Configure(
    IApplicationBuilder app,
    Microsoft.AspNetCore.Routing.IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ChartController.Index",
      areaName: "Mess.Chart",
      pattern: "Chart",
      defaults: new
      {
        controller = typeof(ChartController).ControllerName(),
        action = nameof(ChartController.Index)
      }
    );
  }
}
