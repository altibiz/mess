using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using Mess.Ozds.Controllers;
using OrchardCore.Mvc.Core.Utilities;
using Mess.Ozds.Context;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Client;
using Mess.Ozds.Chart;
using Mess.Chart.Abstractions.Extensions;
using OrchardCore.ContentManagement;
using Mess.Ozds.Abstractions.Models;
using Mess.Iot.Abstractions.Extensions;
using Mess.Ozds.Pushing;
using Mess.Ozds.Indexes;
using Mess.Ozds.Security;
using Mess.Billing.Abstractions.Extensions;
using Mess.Ozds.Billing;

namespace Mess.Ozds;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();

    services.AddTimeseriesDbContext<OzdsDbContext>();

    services.AddSingleton<IOzdsClient, OzdsClient>();
    services.AddSingleton<IOzdsQuery>(
      services => services.GetRequiredService<IOzdsClient>()
    );

    services.AddContentPart<OzdsMeasurementDevicePart>();
    services.AddIndexProvider<DistributionSystemUnitIndexProvider>();
    services.AddIndexProvider<OzdsMeasurementDeviceClosedDistributionSystemIndexProvider>();
    services.AddIndexProvider<OzdsMeasurementDeviceDistributionSystemOperatorIndexProvider>();
    services.AddIndexProvider<OzdsMeasurementDeviceDistributionSystemUnitIndexProvider>();

    services.AddContentPart<PidgeonMeasurementDevicePart>();
    services.AddMeasurementDevicePushHandler<PidgeonPushHandler>();
    services.AddMeasurementDeviceAuthorizationHandler<PidgeonAuthorizationHandler>();

    services.AddContentPart<AbbMeasurementDevicePart>();
    services.AddMeasurementDevicePushHandler<AbbPushHandler>();
    services.AddChartProvider<AbbChartProvider>();
    services.AddBillingFactory<AbbBillingFactory>();

    services.AddContentPart<DistributionSystemOperatorPart>();
    services.AddContentPart<ClosedDistributionSystemPart>();
    services.AddContentPart<DistributionSystemUnitPart>();

    services.AddContentPart<OperatorCataloguePart>();
    services.AddContentPart<RegulatoryAgencyCataloguePart>();

    services.AddContentPart<OzdsCalculationPart>();
    services.AddContentPart<OzdsReceiptPart>();
    services.AddContentPart<OzdsInvoicePart>();
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
      name: "Mess.Ozds.MeasurementDeviceAdminController.List",
      areaName: "Mess.Ozds",
      pattern: adminUrlPrefix + "/Devices",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceAdminController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceAdminController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.MeasurementDeviceAdminController.Detail",
      areaName: "Mess.Ozds",
      pattern: adminUrlPrefix + "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceAdminController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceAdminController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.MeasurementDeviceController.List",
      areaName: "Mess.Ozds",
      pattern: "/Devices",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.MeasurementDeviceController.Detail",
      areaName: "Mess.Ozds",
      pattern: "/Devices/{contentItemId}",
      defaults: new
      {
        controller = typeof(OzdsMeasurementDeviceController).ControllerName(),
        action = nameof(OzdsMeasurementDeviceController.Detail)
      }
    );

    app.UseEndpoints(endpoints =>
    {
      endpoints.Redirect("/", "/Devices");
    });
  }
}
