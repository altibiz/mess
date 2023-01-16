using Mess.MeasurementDevice.Abstractions.Parsers.Egauge;
using Mess.MeasurementDevice.Controllers;
using Mess.MeasurementDevice.Parsers.Egauge;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ResourceManagement;

namespace Mess.MeasurementDevice;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();

    services.AddSingleton<IEgaugeParser, EgaugeParser>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    routes.MapAreaControllerRoute(
      name: "Mess.MeasurementDevice.PushController.Egauge",
      areaName: "Mess.MeasurementDevice",
      pattern: $"/push/egauge",
      defaults: new
      {
        controller = typeof(PushController).ControllerName(),
        action = nameof(PushController.Egauge)
      }
    );
  }
}
