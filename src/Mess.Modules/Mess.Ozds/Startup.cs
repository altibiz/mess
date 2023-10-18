using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using Mess.Ozds.Controllers;
using Mess.Ozds.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Chart;
using Mess.Chart.Abstractions.Extensions;
using OrchardCore.ContentManagement;
using Mess.Ozds.Abstractions.Models;
using Mess.Iot.Abstractions.Extensions;
using Mess.Ozds.Iot;
using Mess.Ozds.Indexes;
using Mess.Billing.Abstractions.Extensions;
using Mess.Ozds.Billing;
using Microsoft.AspNetCore.Authorization;
using Mess.Ozds.Security;
using SixLabors.ImageSharp.ColorSpaces.Companding;

namespace Mess.Ozds;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    // Migrations
    services.AddDataMigration<Migrations>();
    services.AddModularTenantEvents<PopulationTenantEvents>();

    // Resources
    services.AddResources<Resources>();

    // Timeseries
    services.AddTimeseriesDbContext<OzdsTimeseriesDbContext>();
    services.AddTimeseriesClient<
      OzdsTimeseriesClient,
      IOzdsTimeseriesClient,
      IOzdsTimeseriesQuery
    >();

    // Billing
    services.AddBillingFactory<AbbBillingFactory>();
    services.AddBillingIndexer<OzdsBillingIndexer>();
    services.AddPaymentIndexer<OzdsPaymentIndexer>();

    // Iot
    services.AddIotPushHandler<PidgeonPushHandler>();
    services.AddIotAuthorizationHandler<PidgeonAuthorizationHandler>();
    services.AddIotPushHandler<AbbPushHandler>();

    // Chart
    services.AddChartFactory<AbbChartProvider>();

    // Ozds
    services.AddAuthorizationHandler<OzdsAuthorizationHandler>();

    services.AddIndexProvider<DistributionSystemUnitIndexProvider>();
    services.AddIndexProvider<ClosedDistributionSystemIndexProvider>();
    services.AddIndexProvider<OperatorCatalogueIndexProvider>();
    services.AddIndexProvider<OzdsIotDeviceIndexProvider>();

    services.AddContentPart<OzdsIotDevicePart>();
    services.AddContentPart<PidgeonIotDevicePart>();
    services.AddContentPart<AbbIotDevicePart>();
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

    app.UseEndpoints(endpoints =>
    {
      endpoints.Redirect("/", "/List/");
    });
  }
}
