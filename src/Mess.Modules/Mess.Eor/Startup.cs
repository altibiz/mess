using Mess.Chart.Abstractions.Extensions.Microsoft;
using Mess.Eor.Chart.Providers;
using Mess.Eor.Controllers;
using Mess.Eor.Indexes;
using Mess.Eor.MeasurementDevice.Pushing;
using Mess.MeasurementDevice.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Msss.Eor.MeasurementDevice.Polling;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Eor.Context;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Client;
using Mess.Eor.Abstractions.Client;
using Mess.Eor.MeasurementDevice.Updating;
using OrchardCore.Environment.Shell.Configuration;
using System.Reflection;
using Mess.Eor.Options;
using Mess.Eor.MeasurementDevice.Security;

namespace Mess.Eor;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    var sectionName = Assembly.GetExecutingAssembly()?.GetName().Name;
    if (sectionName is null)
    {
      throw new InvalidProgramException("Could not get assembly name");
    }

    var configuration = _shellConfiguration.GetSection(sectionName);
    services.Configure<EorOptions>(configuration);

    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();

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
      name: "Mess.Eor.MeasurementDeviceAdminController.Create",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}/Create",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.Create)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.Edit",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}/Edit",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.Edit)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.Delete",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}/Delete",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.Delete)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.ToggleRunState",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}/ToggleRunState",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.ToggleRunState)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.Reset",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}/Reset",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.Reset)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Eor.MeasurementDeviceAdminController.SetMode",
      areaName: "Mess.Eor",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}/SetMode",
      defaults: new
      {
        controller = typeof(EorMeasurementDeviceAdminController).ControllerName(),
        action = nameof(EorMeasurementDeviceAdminController.SetMode)
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

    // TODO: dynamic rewrite rule that will detect whether user is admin or not
    // inject an IRule implementation and add it here
    // app.UseRewriter(
    //   new RewriteOptions()
    //     .AddRewrite(adminUrlPrefix, adminUrlPrefix + "/Devices", false)
    //     .AddRewrite("/", "/Devices", false)
    // );
  }

  public Startup(IShellConfiguration shellConfiguration)
  {
    _shellConfiguration = shellConfiguration;
  }

  private readonly IShellConfiguration _shellConfiguration;
}
