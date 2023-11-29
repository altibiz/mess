using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.ContentManagement;
using Mess.Iot.Controllers;
using Mess.Iot.Abstractions.Models;
using Mess.Iot.Indexes;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Caches;
using Mess.Iot.Abstractions;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;

namespace Mess.Iot;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();

    services.AddResources<Resources>();

    services.AddNavigationProvider<AdminMenu>();
    services.AddPermissionProvider<Permissions>();

    services.AddContentPart<IotDevicePart>();

    services.AddIndexProvider<IotDeviceIndexProvider>();

    services.AddScoped<IIotDeviceContentItemCache, IotDeviceContentItemCache>();
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

    routes.MapAreaControllerRoute<IotDeviceController>(
      nameof(IotDeviceController.Push),
      "/Push/{deviceId}"
    );

    routes.MapAreaControllerRoute<IotDeviceController>(
      nameof(IotDeviceController.Update),
      "/Update/{deviceId}"
    );

    routes.MapAreaControllerRoute<IotDeviceController>(
      nameof(IotDeviceController.Poll),
      "/Poll/{deviceId}"
    );

    routes.MapAreaControllerRoute<AdminController>(
      nameof(AdminController.ListIotDevices),
      adminUrlPrefix + "/IotDevices"
    );
  }
}
