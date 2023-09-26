using Mess.Chart.Abstractions.Extensions.Microsoft;
using Mess.Eor.Chart.Providers;
using Mess.Eor.Controllers;
using Mess.Eor.Indexes;
using Mess.Eor.MeasurementDevice.Pushing;
using Mess.Iot.Abstractions.Extensions.Microsoft;
using Mess.Iot.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mess.Eor.MeasurementDevice.Polling;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Eor.Context;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.Iot.Client;
using Mess.Eor.Abstractions.Client;
using Mess.Eor.MeasurementDevice.Updating;
using OrchardCore.Environment.Shell.Configuration;
using Mess.Eor.MeasurementDevice.Security;
using Mess.Eor.Abstractions;

namespace Mess.Eor;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();
    services.AddNavigationProvider<AdminMenu>();
    services.AddPermissionProvider<EorPermissions>();

    services.AddTimeseriesDbContext<EorTimeseriesDbContext>();
    services.AddSingleton<IEorTimeseriesClient, EorTimeseriesClient>();
    services.AddSingleton<IEorTimeseriesQuery>(
      services => services.GetRequiredService<IEorTimeseriesClient>()
    );

    services.AddContentPart<EorMeasurementDevicePart>();
    services.AddMeasurementDevicePushHandler<EorPushHandler>();
    services.AddMeasurementDevicePollHandler<EorPollHandler>();
    services.AddMeasurementDeviceUpdateHandler<EorUpdateHandler>();
    services.AddMeasurementDeviceAuthorizationHandler<EorAuthorizationHandler>();
    services.AddChartProvider<EorChartProvider>();
    services.AddIndexProvider<EorMeasurementDeviceIndexProvider>();
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
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.Detail",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceController.List",
      areaName: "Mess.Eor",
      pattern: "/Devices",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceController).ControllerName(),
        action = nameof(EorMeasurementDeviceController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceController.Detail",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceController).ControllerName(),
        action = nameof(EorMeasurementDeviceController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceDataController.Data",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Data",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceDataController).ControllerName(),
        action = nameof(EorMeasurementDeviceDataController.Index)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceControlsController.ToggleRunState",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/ToggleRunState",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceControlsController).ControllerName(),
        action = nameof(EorMeasurementDeviceControlsController.ToggleRunState)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceControlsController.Start",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Start",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceControlsController).ControllerName(),
        action = nameof(EorMeasurementDeviceControlsController.Start)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceControlsController.Stop",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Stop",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceControlsController).ControllerName(),
        action = nameof(EorMeasurementDeviceControlsController.Stop)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceControlsController.Reset",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Reset",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceControlsController).ControllerName(),
        action = nameof(EorMeasurementDeviceControlsController.Reset)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceControlsController.SetMode",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/SetMode",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceControlsController).ControllerName(),
        action = nameof(EorMeasurementDeviceControlsController.SetMode)
      }
    );

    app.UseEndpoints(endpoints =>
    {
      endpoints.Redirect("/", "/Devices");
    });
  }

  public Startup(IShellConfiguration shellConfiguration)
  {
    _shellConfiguration = shellConfiguration;
  }

  private readonly IShellConfiguration _shellConfiguration;
}
