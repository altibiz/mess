using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Chart.Controllers;
using Mess.Chart.Drivers;
using Mess.Chart.Abstractions.Models;
using OrchardCore.Admin;
using Mess.Chart.Providers;
using Mess.Chart.Indexes;
using Mess.Chart.Abstractions.Extensions;
using Mess.OrchardCore.Extensions.Microsoft;

namespace Mess.Chart;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();

    services.AddIndexProvider<ChartIndexProvider>();
    services.AddChartFactory<PreviewChartFactory>();
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
      pattern: "/Chart/{contentItemId}",
      defaults: new
      {
        controller = typeof(ChartController).ControllerName(),
        action = nameof(ChartController.Index)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Chart.Controllers.ChartController.Preview",
      areaName: "Mess.Chart",
      pattern: "/Chart/Preview/{contentItemId}",
      defaults: new
      {
        controller = typeof(ChartController).ControllerName(),
        action = nameof(ChartController.Preview)
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
