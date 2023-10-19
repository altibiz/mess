using Mess.Chart.Abstractions.Extensions;
using Mess.Eor.Chart;
using Mess.Eor.Controllers;
using Mess.Eor.Indexes;
using Mess.Eor.Iot;
using Mess.Iot.Abstractions.Extensions;
using Mess.Eor.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Eor.Abstractions.Timeseries;
using OrchardCore.Environment.Shell.Configuration;
using Mess.Eor.Abstractions;

namespace Mess.Eor;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    // Migrations
    services.AddDataMigration<Migrations>();

    // Resources
    services.AddResources<Resources>();

    // Navigation
    services.AddNavigationProvider<AdminMenu>();

    // Permissions
    services.AddPermissionProvider<EorPermissions>();

    // Populations
    // services.AddPopulation<Populations>();

    // Contents
    services.AddContentPart<EorIotDevicePart>();

    // Indexing
    services.AddIndexProvider<EorIotDeviceIndexProvider>();

    // Timeseries
    services.AddTimeseriesDbContext<EorTimeseriesDbContext>();
    services.AddTimeseriesClient<
      EorTimeseriesClient,
      IEorTimeseriesClient,
      IEorTimeseriesQuery
    >();

    // Iot
    services.AddIotPushHandler<EorPushHandler>();
    services.AddIotPollHandler<EorPollHandler>();
    services.AddIotUpdateHandler<EorUpdateHandler>();
    services.AddIotAuthorizationHandler<EorAuthorizationHandler>();

    // Chart
    services.AddChartFactory<EorIotDeviceChartProvider>();
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
      name: "Mess.Eor.EorIotDeviceAdminController.List",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices",
      defaults: new
      {
        controller = typeof(EorIotDeviceAdminController).ControllerName(),
        action = nameof(EorIotDeviceAdminController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceAdminController.Detail",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(EorIotDeviceAdminController).ControllerName(),
        action = nameof(EorIotDeviceAdminController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceController.List",
      areaName: "Mess.Eor",
      pattern: "/Devices",
      defaults: new
      {
        controller = typeof(EorIotDeviceController).ControllerName(),
        action = nameof(EorIotDeviceController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceController.Detail",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(EorIotDeviceController).ControllerName(),
        action = nameof(EorIotDeviceController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceDataController.Data",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Data",
      defaults: new
      {
        controller = typeof(EorIotDeviceDataController).ControllerName(),
        action = nameof(EorIotDeviceDataController.Index)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceControlsController.ToggleRunState",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/ToggleRunState",
      defaults: new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.ToggleRunState)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceControlsController.Start",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Start",
      defaults: new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.Start)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceControlsController.Stop",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Stop",
      defaults: new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.Stop)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceControlsController.Reset",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/Reset",
      defaults: new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.Reset)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.EorIotDeviceControlsController.SetMode",
      areaName: "Mess.Eor",
      pattern: "/Devices/{contentItemId}/SetMode",
      defaults: new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.SetMode)
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
