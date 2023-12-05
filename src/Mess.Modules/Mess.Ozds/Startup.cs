using Mess.Billing.Abstractions.Extensions;
using Mess.Chart.Abstractions.Extensions;
using Mess.Cms.Extensions.Microsoft;
using Mess.Iot.Abstractions.Extensions;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Billing;
using Mess.Ozds.Chart;
using Mess.Ozds.Controllers;
using Mess.Ozds.Indexes;
using Mess.Ozds.Iot;
using Mess.Ozds.Security;
using Mess.Ozds.Timeseries;
using Mess.Population.Abstractions.Extensions;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Mess.Ozds;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    // Migrations
    services.AddDataMigration<Migrations>();

    // Resources
    services.AddResources<Resources>();

    // Navigation
    services.AddNavigationProvider<AdminMenu>();

    // Authorization
    services.AddAuthorizationHandler<OzdsAuthorizationHandler>();

    // Contents
    services.AddContentPart<DistributionSystemOperatorPart>();
    services.AddContentPart<ClosedDistributionSystemPart>();
    services.AddContentPart<DistributionSystemUnitPart>();
    services.AddContentPart<OperatorCataloguePart>();
    services.AddContentPart<RegulatoryAgencyCataloguePart>();
    services.AddContentPart<OzdsCalculationPart>();
    services.AddContentPart<OzdsReceiptPart>();
    services.AddContentPart<OzdsInvoicePart>();
    services.AddContentPart<OzdsIotDevicePart>();
    services.AddContentPart<PidgeonIotDevicePart>();
    services.AddContentPart<AbbIotDevicePart>();
    services.AddContentPart<SchneiderIotDevicePart>();

    // Indexing
    services.AddIndexProvider<DistributionSystemUnitIndexProvider>();
    services.AddIndexProvider<ClosedDistributionSystemIndexProvider>();
    services.AddIndexProvider<OperatorCatalogueIndexProvider>();
    services.AddIndexProvider<OzdsIotDeviceIndexProvider>();

    // Populations
    services.AddPopulation<Populations>();

    // Timeseries
    services.AddTimeseriesDbContext<OzdsTimeseriesDbContext>();
    services.AddTimeseriesClient<
      OzdsTimeseriesClient,
      IOzdsTimeseriesClient,
      IOzdsTimeseriesQuery
    >();

    // Billing
    services.AddBillingIndexer<OzdsBillingIndexer>();
    services.AddPaymentIndexer<OzdsPaymentIndexer>();
    services.AddBillingFactory<AbbBillingFactory>();
    services.AddBillingFactory<SchneiderBillingFactory>();

    // Iot
    services.AddIotPushHandler<PidgeonPushHandler>();
    services.AddIotAuthorizationHandler<PidgeonAuthorizationHandler>();
    services.AddIotPushHandler<AbbPushHandler>();
    services.AddIotPushHandler<SchneiderPushHandler>();

    // Chart
    services.AddChartFactory<AbbChartFactory>();
    services.AddChartFactory<SchneiderChartFactory>();
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

    routes.MapAreaControllerRoute<DistributionSystemUnitAdminController>(
      nameof(DistributionSystemUnitAdminController.List),
      adminUrlPrefix + "/DistributionSystemUnit/List"
    );
    routes.MapAreaControllerRoute<DistributionSystemUnitAdminController>(
      nameof(DistributionSystemUnitAdminController.Detail),
      adminUrlPrefix + "/DistributionSystemUnit/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<ClosedDistributionSystemAdminController>(
      nameof(ClosedDistributionSystemAdminController.List),
      adminUrlPrefix + "/ClosedDistributionSystem/List"
    );
    routes.MapAreaControllerRoute<ClosedDistributionSystemAdminController>(
      nameof(ClosedDistributionSystemAdminController.Detail),
      adminUrlPrefix + "/ClosedDistributionSystem/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<OzdsIotDeviceAdminController>(
      nameof(OzdsIotDeviceAdminController.List),
      adminUrlPrefix + "/OzdsIotDevice/List"
    );
    routes.MapAreaControllerRoute<OzdsIotDeviceAdminController>(
      nameof(OzdsIotDeviceAdminController.Detail),
      adminUrlPrefix + "/OzdsIotDevice/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<DistributionSystemOperatorAdminController>(
      nameof(DistributionSystemOperatorAdminController.List),
      adminUrlPrefix + "/DistributionSystemOperator/List"
    );
    routes.MapAreaControllerRoute<DistributionSystemOperatorAdminController>(
      nameof(DistributionSystemOperatorAdminController.Detail),
      adminUrlPrefix + "/DistributionSystemOperator/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<DistributionSystemUnitController>(
      nameof(DistributionSystemUnitController.List),
      "/DistributionSystemUnit/List"
    );
    routes.MapAreaControllerRoute<DistributionSystemUnitController>(
      nameof(DistributionSystemUnitController.Detail),
      "/DistributionSystemUnit/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<ClosedDistributionSystemController>(
      nameof(ClosedDistributionSystemController.List),
      "/ClosedDistributionSystem/List"
    );
    routes.MapAreaControllerRoute<ClosedDistributionSystemController>(
      nameof(ClosedDistributionSystemController.Detail),
      "/ClosedDistributionSystem/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<DistributionSystemOperatorController>(
      nameof(DistributionSystemOperatorController.List),
      "/DistributionSystemOperator/List"
    );
    routes.MapAreaControllerRoute<DistributionSystemOperatorController>(
      nameof(DistributionSystemOperatorController.Detail),
      "/DistributionSystemOperator/Detail/{contentItemId}"
    );

    routes.MapAreaControllerRoute<OzdsIotDeviceController>(
      nameof(OzdsIotDeviceController.List),
      "/OzdsIotDevice/List"
    );
    routes.MapAreaControllerRoute<OzdsIotDeviceController>(
      nameof(OzdsIotDeviceController.Detail),
      "/OzdsIotDevice/Detail/{contentItemId}"
    );

    app.UseEndpoints(endpoints =>
    {
      endpoints.Redirect("/", "/OzdsIotDevice/List");
    });
  }
}
