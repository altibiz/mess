using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ResourceManagement;
using Mess.Chart.Controllers;
using Mess.Chart.Drivers;
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
      .AddContentPart<TimeseriesChartPart>()
      .UseDisplayDriver<TimeseriesChartPartDisplayDriver>();

    services
      .AddContentPart<TimeseriesChartDatasetPart>()
      .UseDisplayDriver<TimeseriesChartDatasetPartDisplayDriver>();
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
      name: "Mess.Chart.Controllers.TimeseriesChartDatasetAdminController.Create",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/TimeseriesChartDataset/Create",
      defaults: new
      {
        controller = typeof(TimeseriesChartDatasetAdminController).ControllerName(),
        action = nameof(TimeseriesChartDatasetAdminController.Create)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.TimeseriesChartDatasetAdminController.Edit",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/TimeseriesChartDataset/Edit",
      defaults: new
      {
        controller = typeof(TimeseriesChartDatasetAdminController).ControllerName(),
        action = nameof(TimeseriesChartDatasetAdminController.Edit)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.TimeseriesChartDatasetAdminController.Delete",
      areaName: "Mess.Chart",
      pattern: _adminOptions.AdminUrlPrefix + "/TimeseriesChartDataset/Delete",
      defaults: new
      {
        controller = typeof(TimeseriesChartDatasetAdminController).ControllerName(),
        action = nameof(TimeseriesChartDatasetAdminController.Delete)
      }
    );
  }

  public Startup(IOptions<AdminOptions> adminOptions)
  {
    _adminOptions = adminOptions.Value;
  }

  private readonly AdminOptions _adminOptions;
}
