using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ContentManagement;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Iot.Abstractions.Timeseries;
using Mess.Iot.Timeseries;
using Mess.Iot.Controllers;
using Mess.Iot.Abstractions.Models;
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

    services.AddResources<Resources>();

    services.AddTimeseriesDbContext<IotTimeseriesDbContext>();
    services.AddTimeseriesClient<
      IotTimeseriesClient,
      IIotTimeseriesClient,
      IIotTimeseriesQuery
    >();

    services.AddContentPart<IotDevicePart>();
    services.AddContentPart<EgaugeIotDevicePart>();

    services.AddIndexProvider<IotDeviceIndexProvider>();

    services.AddScoped<IIotDeviceContentItemCache, IotDeviceContentItemCache>();
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
