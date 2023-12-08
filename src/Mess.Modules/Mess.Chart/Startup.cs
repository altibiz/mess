using Mess.Chart.Abstractions.Extensions;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Controllers;
using Mess.Chart.Drivers;
using Mess.Chart.Factories;
using Mess.Chart.Indexes;
using Mess.Cms.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.Chart;

public class Startup : StartupBase
{
  private readonly AdminOptions _adminOptions;

  public Startup(IOptions<AdminOptions> adminOptions)
  {
    _adminOptions = adminOptions.Value;
  }

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
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    routes.MapAreaControllerRoute(
      "Mess.Chart.Controllers.ChartController.Index",
      "Mess.Chart",
      "/Chart/{contentItemId}",
      new
      {
        controller = typeof(ChartController).ControllerName(),
        action = nameof(ChartController.Index)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Chart.Controllers.ChartController.Preview",
      "Mess.Chart",
      "/Chart/Preview/{contentItemId}",
      new
      {
        controller = typeof(ChartController).ControllerName(),
        action = nameof(ChartController.Preview)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Chart.Controllers.TimeseriesChartDatasetAdminController.Create",
      "Mess.Chart",
      _adminOptions.AdminUrlPrefix + "/TimeseriesChartDataset/Create",
      new
      {
        controller =
          typeof(TimeseriesChartDatasetAdminController).ControllerName(),
        action = nameof(TimeseriesChartDatasetAdminController.Create)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Chart.Controllers.TimeseriesChartDatasetAdminController.Edit",
      "Mess.Chart",
      _adminOptions.AdminUrlPrefix + "/TimeseriesChartDataset/Edit",
      new
      {
        controller =
          typeof(TimeseriesChartDatasetAdminController).ControllerName(),
        action = nameof(TimeseriesChartDatasetAdminController.Edit)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Chart.Controllers.TimeseriesChartDatasetAdminController.Delete",
      "Mess.Chart",
      _adminOptions.AdminUrlPrefix + "/TimeseriesChartDataset/Delete",
      new
      {
        controller =
          typeof(TimeseriesChartDatasetAdminController).ControllerName(),
        action = nameof(TimeseriesChartDatasetAdminController.Delete)
      }
    );
  }
}
