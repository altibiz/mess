using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ResourceManagement;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Controllers;
using Mess.Chart.Drivers;
using Mess.Chart.Providers;
using OrchardCore.Security.Permissions;
using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using OrchardCore.Admin;

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

    services.AddScoped<IPermissionProvider, ChartPermissions>();

    services
      .AddContentPart<ChartPart>()
      .UseDisplayDriver<ChartPartDisplayDriver>();

    services
      .AddContentPart<LineChartPart>()
      .UseDisplayDriver<LineChartPartDisplayDriver>();

    services
      .AddContentPart<LineChartDatasetPart>()
      .UseDisplayDriver<LineChartDatasetPartDisplayDriver>();

    services
      .AddContentPart<TimeseriesChartDatasetPart>()
      .UseDisplayDriver<TimeseriesChartDatasetPartDisplayDriver>();

    services.AddSingleton<IChartDataProviderLookup, ChartProviderLookup>();
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
      pattern: "/Chart",
      defaults: new
      {
        controller = typeof(ChartController).ControllerName(),
        action = nameof(ChartController.Index)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.AdminController.Index",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.Index)
      }
    );
  }

  public Startup(IOptions<AdminOptions> adminOptions)
  {
    _adminOptions = adminOptions.Value;
  }

  private readonly AdminOptions _adminOptions;
}
