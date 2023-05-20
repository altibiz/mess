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
using Mess.MeasurementDevice.Models;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.MeasurementDevice.Dispatchers;
using Mess.MeasurementDevice.Abstractions.Models;

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

    services.AddContentPart<EgaugeMeasurementDevicePart>();

    services.AddTimeseriesDbContext<MeasurementDbContext>();

    services.AddScoped<IMeasurementDispatcher, EgaugeMeasurementDispatcher>();

    services.AddSingleton<IMeasurementClient, MeasurementClient>();
    services.AddSingleton<IMeasurementQuery>(
      services => services.GetRequiredService<IMeasurementClient>()
    );

    services.AddContentPart<MeasurementDevicePart>();
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
      pattern: "/Push/{dispatcherId}",
      defaults: new
      {
        controller = typeof(PushController).ControllerName(),
        action = nameof(PushController.Index)
      }
    );
  }
}
