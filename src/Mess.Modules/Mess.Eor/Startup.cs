using Mess.Chart.Abstractions.Extensions;
using Mess.Cms.Extensions.Microsoft;
using Mess.Eor.Abstractions;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Chart;
using Mess.Eor.Controllers;
using Mess.Eor.Indexes;
using Mess.Eor.Iot;
using Mess.Eor.Localization;
using Mess.Eor.Localization.Abstractions;
using Mess.Iot.Abstractions.Extensions;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.Eor;

public class Startup : StartupBase
{
  private readonly IShellConfiguration _shellConfiguration;

  public Startup(IShellConfiguration shellConfiguration)
  {
    _shellConfiguration = shellConfiguration;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    // Localization
    services.AddSingleton<IEorLocalizer, EorLocalizer>();

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
    services.AddChartFactory<EorChartProvider>();
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
      "Mess.Eor.EorIotDeviceAdminController.List",
      "Mess.Eor",
      adminUrlPrefix + "/Devices",
      new
      {
        controller = typeof(EorIotDeviceAdminController).ControllerName(),
        action = nameof(EorIotDeviceAdminController.List)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceAdminController.Detail",
      "Mess.Eor",
      adminUrlPrefix + "/Devices/{contentItemId}",
      new
      {
        controller = typeof(EorIotDeviceAdminController).ControllerName(),
        action = nameof(EorIotDeviceAdminController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceController.List",
      "Mess.Eor",
      "/Devices",
      new
      {
        controller = typeof(EorIotDeviceController).ControllerName(),
        action = nameof(EorIotDeviceController.List)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceController.Detail",
      "Mess.Eor",
      "/Devices/{contentItemId}",
      new
      {
        controller = typeof(EorIotDeviceController).ControllerName(),
        action = nameof(EorIotDeviceController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceDataController.Data",
      "Mess.Eor",
      "/Devices/{contentItemId}/Data",
      new
      {
        controller = typeof(EorIotDeviceDataController).ControllerName(),
        action = nameof(EorIotDeviceDataController.Index)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceControlsController.ToggleRunState",
      "Mess.Eor",
      "/Devices/{contentItemId}/ToggleRunState",
      new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.ToggleRunState)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceControlsController.Start",
      "Mess.Eor",
      "/Devices/{contentItemId}/Start",
      new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.Start)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceControlsController.Stop",
      "Mess.Eor",
      "/Devices/{contentItemId}/Stop",
      new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.Stop)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceControlsController.Reset",
      "Mess.Eor",
      "/Devices/{contentItemId}/Reset",
      new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.Reset)
      }
    );

    routes.MapAreaControllerRoute(
      "Mess.Eor.EorIotDeviceControlsController.SetMode",
      "Mess.Eor",
      "/Devices/{contentItemId}/SetMode",
      new
      {
        controller = typeof(EorIotDeviceControlsController).ControllerName(),
        action = nameof(EorIotDeviceControlsController.SetMode)
      }
    );

    app.UseEndpoints(endpoints => { endpoints.Redirect("/", "/Devices"); });
  }
}
