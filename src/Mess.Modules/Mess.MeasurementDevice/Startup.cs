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
using Mess.MeasurementDevice.Parsers.Egauge;
using Mess.MeasurementDevice.Models;
using Mess.MeasurementDevice.Abstractions.Parsers;
using Mess.MeasurementDevice.Storage;
using Mess.MeasurementDevice.Abstractions.Storage;
using Mess.MeasurementDevice.Parsers;

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

    services.RegisterTimeseriesDbContext<MeasurementDbContext>();

    services.AddSingleton<IMeasurementParserLookup, MeasurementParserLookup>();
    services.AddSingleton<IMeasurementParser, EgaugeParser>();

    services.AddSingleton<
      IMeasurementStorageStrategyLookup,
      MeasurementStorageStrategyLookup
    >();
    services.AddSingleton<
      IMeasurementStorageStrategy,
      EgaugeDirectStorageStrategy
    >();

    services.AddSingleton<IMeasurementClient, MeasurementClient>();

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
      pattern: "/Push/{parserId}",
      defaults: new
      {
        controller = typeof(PushController).ControllerName(),
        action = nameof(PushController.Index)
      }
    );
  }
}
