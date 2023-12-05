using Mess.Chart.Abstractions.Extensions;
using Mess.Cms.Extensions.Microsoft;
using Mess.Enms.Abstractions.Models;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Enms.Chart;
using Mess.Enms.Iot;
using Mess.Enms.Timeseries;
using Mess.Iot.Abstractions.Extensions;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Mess.Enms;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    // Migrations
    services.AddDataMigration<Migrations>();

    // Resources
    services.AddResources<Resources>();

    // Populations
    // services.AddPopulation<Populations>();

    // Content
    services.AddContentPart<EgaugeIotDevicePart>();

    // Timeseries
    services.AddTimeseriesDbContext<EnmsTimeseriesDbContext>();
    services.AddTimeseriesClient<
      EnmsTimeseriesClient,
      IEnmsTimeseriesClient,
      IEnmsTimeseriesQuery
    >();

    // Chart
    services.AddChartFactory<EgaugeChartFactory>();

    // Iot
    services.AddIotPushHandler<EgaugePushHandler>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
  }
}
