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
using Mess.Iot.Abstractions.Client;
using Mess.Iot.Client;
using Mess.Iot.Controllers;
using Mess.Iot.Pushing;
using Mess.Iot.Abstractions.Models;
using Mess.Iot.Abstractions.Extensions;
using Mess.Iot.Indexes;
using YesSql.Indexes;
using Mess.Iot.Context;
using Mess.Iot.Abstractions.Services;
using Mess.Iot.Services;

namespace Mess.Iot;

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

    services.AddScoped<
      IMeasurementDeviceContentItemCache,
      MeasurementDeviceContentItemCache
    >();

    services.AddContentPart<EgaugeMeasurementDevicePart>();
    services.AddMeasurementDevicePushHandler<EgaugePushHandler>();
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
        controller = typeof(DeviceController).ControllerName(),
        action = nameof(DeviceController.Push)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Iot.UpdateController.Index",
      areaName: "Mess.Iot",
      pattern: "/Update/{deviceId}",
      defaults: new
      {
        controller = typeof(DeviceController).ControllerName(),
        action = nameof(DeviceController.Update)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Iot.PollController.Index",
      areaName: "Mess.Iot",
      pattern: "/Poll/{deviceId}",
      defaults: new
      {
        controller = typeof(DeviceController).ControllerName(),
        action = nameof(DeviceController.Poll)
      }
    );
  }
}
