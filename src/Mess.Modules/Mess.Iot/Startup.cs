using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ResourceManagement;
using OrchardCore.ContentManagement;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Iot.Abstractions.Timeseries;
using Mess.Iot.Timeseries;
using Mess.Iot.Controllers;
using Mess.Iot.Iot;
using Mess.Iot.Abstractions.Models;
using Mess.Iot.Abstractions.Extensions;
using Mess.Iot.Indexes;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Services;
using Mess.OrchardCore.Extensions.Microsoft;

namespace Mess.Iot;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    // FIXME: singleton
    services.AddModularTenantEvents<Populations>();
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();

    services.AddContentPart<IotDevicePart>();

    services.AddTimeseriesDbContext<IotTimeseriesDbContext>();
    services.AddTimeseriesClient<
      IotTimeseriesClient,
      IIotTimeseriesClient,
      IIotTimeseriesQuery
    >();

    services.AddIndexProvider<IotDeviceIndexProvider>();

    services.AddScoped<IIotDeviceContentItemCache, IotDeviceContentItemCache>();

    services.AddContentPart<EgaugeIotDevicePart>();
    services.AddIotPushHandler<EgaugePushHandler>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    routes.MapAreaControllerRoute(
      name: "Mess.Iot.PushController.Index",
      areaName: "Mess.Iot",
      pattern: "/Push/{deviceId}",
      defaults: new
      {
        controller = typeof(IotDeviceController).ControllerName(),
        action = nameof(IotDeviceController.Push)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Iot.UpdateController.Index",
      areaName: "Mess.Iot",
      pattern: "/Update/{deviceId}",
      defaults: new
      {
        controller = typeof(IotDeviceController).ControllerName(),
        action = nameof(IotDeviceController.Update)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Iot.PollController.Index",
      areaName: "Mess.Iot",
      pattern: "/Poll/{deviceId}",
      defaults: new
      {
        controller = typeof(IotDeviceController).ControllerName(),
        action = nameof(IotDeviceController.Poll)
      }
    );
  }
}
