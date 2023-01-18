using Mess.Chart.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Chart.Controllers;
using Mess.MeasurementDevice.Chart.Models;
using Mess.MeasurementDevice.Chart.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.MeasurementDevice.Chart;

[RequireFeatures("Mess.Chart")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();

    services.AddContentPart<EgaugeChartFieldPart>();

    services.RegisterChartProvider<EgaugeChartProvider>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    routes.MapAreaControllerRoute(
      name: "Mess.MeasuermentDevice.Chart.Controllers.AdminController.AddEgaugeChartField",
      areaName: "Mess.MeasurementDevice",
      pattern: _adminOptions.AdminUrlPrefix
        + "/MeasurementDevice/AddEgaugeChartField/{id}",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.AddEgaugeChartField)
      }
    );
  }

  public Startup(IOptions<AdminOptions> adminOptions)
  {
    _adminOptions = adminOptions.Value;
  }

  private readonly AdminOptions _adminOptions;
}
