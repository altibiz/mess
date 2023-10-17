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
using Mess.Ozds.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Ozds.Abstractions.Timeseries;
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
using Mess.Ozds.Handlers;

namespace Mess.Ozds;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();

    services.AddTimeseriesDbContext<OzdsTimeseriesDbContext>();
    services.AddTimeseriesClient<
      OzdsTimeseriesClient,
      IOzdsTimeseriesClient,
      IOzdsTimeseriesQuery
    >();

    services.AddContentPart<OzdsIotDevicePart>();
    services.AddIndexProvider<DistributionSystemUnitIndexProvider>();
    services.AddIndexProvider<OzdsIotDeviceClosedDistributionSystemIndexProvider>();
    services.AddIndexProvider<OzdsIotDeviceDistributionSystemOperatorIndexProvider>();
    services.AddIndexProvider<OzdsIotDeviceDistributionSystemUnitIndexProvider>();
    services.AddContentHandler<OzdsIotDeviceHandler>();

    services.AddContentPart<PidgeonIotDevicePart>();
    services.AddIotPushHandler<PidgeonPushHandler>();
    services.AddIotAuthorizationHandler<PidgeonAuthorizationHandler>();

    services.AddContentPart<AbbIotDevicePart>();
    services.AddIotPushHandler<AbbPushHandler>();
    services.AddChartFactory<AbbChartProvider>();
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

    routes.MapAreaControllerRoute<DistributionSystemUnitController>(
      nameof(DistributionSystemUnitController.List),
      adminUrlPrefix + "/DistributionSystemUnit/List/{contentType}"
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.AdminController.List",
      areaName: "Mess.Ozds",
      pattern: adminUrlPrefix + "/List/{contentType}",
      defaults: new
      {
        controller = typeof(DistributionSystemUnitController).ControllerName(),
        action = nameof(DistributionSystemUnitController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.AdminController.Detail",
      areaName: "Mess.Ozds",
      pattern: adminUrlPrefix + "/Device/{contentItemId}",
      defaults: new
      {
        controller = typeof(DistributionSystemUnitController).ControllerName(),
        action = nameof(DistributionSystemUnitController.Detail)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.OzdsController.List",
      areaName: "Mess.Ozds",
      pattern: "/Devices",
      defaults: new
      {
        controller = typeof(IotDeviceController).ControllerName(),
        action = nameof(IotDeviceController.List)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Ozds.IotDeviceController.Detail",
      areaName: "Mess.Ozds",
      pattern: "/Detail/{contentItemId}",
      defaults: new
      {
        controller = typeof(IotDeviceController).ControllerName(),
        action = nameof(IotDeviceController.Detail)
      }
    );

    app.UseEndpoints(endpoints =>
    {
      endpoints.Redirect("/", "/List/");
    });
  }
}
