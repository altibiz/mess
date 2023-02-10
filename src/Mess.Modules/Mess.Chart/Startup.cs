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
using Mess.Chart.Abstractions.Services;
using Mess.Chart.Services;

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
    services.AddScoped<IChartService, ChartService>();

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
      name: "Mess.Chart.Controllers.ConcreteChartAdminController.Create",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/ConcreteChart/Create",
      defaults: new
      {
        controller = typeof(ConcreteChartAdminController).ControllerName(),
        action = nameof(ConcreteChartAdminController.Create)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ConcreteChartAdminController.Edit",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/ConcreteChart/Edit",
      defaults: new
      {
        controller = typeof(ConcreteChartAdminController).ControllerName(),
        action = nameof(ConcreteChartAdminController.Edit)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ConcreteChartAdminController.Delete",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/ConcreteChart/Delete",
      defaults: new
      {
        controller = typeof(ConcreteChartAdminController).ControllerName(),
        action = nameof(ConcreteChartAdminController.Delete)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ConcreteLineChartDatasetAdminController.Create",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix
        + "/ConcreteLineChartDataset/Create",
      defaults: new
      {
        controller = typeof(ConcreteLineChartDatasetAdminController).ControllerName(),
        action = nameof(ConcreteLineChartDatasetAdminController.Create)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ConcreteLineChartDatasetAdminController.Edit",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/ConcreteLineChartDataset/Edit",
      defaults: new
      {
        controller = typeof(ConcreteLineChartDatasetAdminController).ControllerName(),
        action = nameof(ConcreteLineChartDatasetAdminController.Edit)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ConcreteLineChartDatasetAdminController.Delete",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix
        + "/ConcreteLineChartDataset/Delete",
      defaults: new
      {
        controller = typeof(ConcreteLineChartDatasetAdminController).ControllerName(),
        action = nameof(ConcreteLineChartDatasetAdminController.Delete)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.LineChartDatasetAdminController.Create",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/LineChartDataset/Create",
      defaults: new
      {
        controller = typeof(LineChartDatasetAdminController).ControllerName(),
        action = nameof(LineChartDatasetAdminController.Create)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.LineChartDatasetAdminController.Edit",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/LineChartDataset/Edit",
      defaults: new
      {
        controller = typeof(LineChartDatasetAdminController).ControllerName(),
        action = nameof(LineChartDatasetAdminController.Edit)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.LineChartDatasetAdminController.Delete",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/LineChartDataset/Delete",
      defaults: new
      {
        controller = typeof(LineChartDatasetAdminController).ControllerName(),
        action = nameof(LineChartDatasetAdminController.Delete)
      }
    );
  }

  public Startup(IOptions<AdminOptions> adminOptions)
  {
    _adminOptions = adminOptions.Value;
  }

  private readonly AdminOptions _adminOptions;
}
