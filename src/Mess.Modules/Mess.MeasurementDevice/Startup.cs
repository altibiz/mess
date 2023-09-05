using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.ResourceManagement;
using OrchardCore.ContentManagement;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Client;
using Mess.MeasurementDevice.Controllers;
using Mess.MeasurementDevice.Pushing;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Indexes;
using YesSql.Indexes;
using Mess.MeasurementDevice.Context;
using Mess.MeasurementDevice.Abstractions.Services;
using Mess.MeasurementDevice.Services;
using Mess.MeasurementDevice.Security;

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

    services.AddContentPart<MeasurementDevicePart>();

    services.AddTimeseriesDbContext<MeasurementDbContext>();

    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();
    services.AddSingleton<ITimeseriesQuery>(
      services => services.GetRequiredService<ITimeseriesClient>()
    );

    services.AddSingleton<IIndexProvider, MeasurementDeviceIndexProvider>();

    services.AddSingleton<
      IMeasurementDeviceContentItemCache,
      MeasurementDeviceContentItemCache
    >();

    services.AddContentPart<EgaugeMeasurementDevicePart>();
    services.AddMeasurementDevicePushHandler<EgaugePushHandler>();

    services.AddContentPart<RaspberryPiMeasurementDevicePart>();
    services.AddMeasurementDevicePushHandler<RaspberryPiPushHandler>();
    services.AddMeasurementDeviceAuthorizationHandler<RaspberryPiAuthorizationHandler>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    routes.MapAreaControllerRoute(
      name: "Mess.MeasurementDevice.PushController.Index",
      areaName: "Mess.MeasurementDevice",
      pattern: "/Push/{deviceId}",
      defaults: new
      {
        controller = typeof(DeviceController).ControllerName(),
        action = nameof(DeviceController.Push)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.MeasurementDevice.UpdateController.Index",
      areaName: "Mess.MeasurementDevice",
      pattern: "/Update/{deviceId}",
      defaults: new
      {
        controller = typeof(DeviceController).ControllerName(),
        action = nameof(DeviceController.Update)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.MeasurementDevice.PollController.Index",
      areaName: "Mess.MeasurementDevice",
      pattern: "/Poll/{deviceId}",
      defaults: new
      {
        controller = typeof(DeviceController).ControllerName(),
        action = nameof(DeviceController.Poll)
      }
    );
  }
}
