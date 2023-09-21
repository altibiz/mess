using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using Mess.Ozds.Controllers;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.Ozds;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    var adminUrlPrefix = app.ApplicationServices
      .GetRequiredService<IOptions<AdminOptions>>()
      .Value.AdminUrlPrefix;

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.List",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceAdminController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceAdminController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.Detail",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceAdminController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceAdminController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceController.List",
      areaName: "Mess.Eor",
      pattern: "/Devices",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceController.Detail",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceController.Detail)
      }
    );

    app.UseEndpoints(endpoints =>
    {
      endpoints.Redirect("/", "/Devices");
    });
  }
}
