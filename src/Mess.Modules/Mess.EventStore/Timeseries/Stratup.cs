using Marten;
using Mess.EventStore.Controllers;
using Mess.EventStore.Extensions.Marten;
using Mess.Tenants;
using Mess.Timeseries.Abstractions.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.EventStore.Timeseries;

[RequireFeatures("Mess.Timeseries")]
public class TimeseriesStartup : StartupBase
{
  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider services
  )
  {
    var timeseriesProjection =
      services.GetRequiredService<TimeseriesProjection>();
    timeseriesProjection.Tenants =
      services.GetRequiredService<ITenantProvider>();
    timeseriesProjection.Client =
      services.GetRequiredService<ITimeseriesClient>();
    timeseriesProjection.Logger = services.GetRequiredService<
      ILogger<TimeseriesProjection>
    >();

    services
      .GetRequiredService<IDocumentStore>()
      .ShowDaemonDiagnostics(
        services.GetRequiredService<ILogger<IDocumentStore>>()
      );

    routes.MapAreaControllerRoute(
      name: "Mess.EventStore.Push.Egauge",
      areaName: "Mess.EventStore",
      pattern: "push/egauge",
      defaults: new
      {
        controller = typeof(PushController).ControllerName(),
        action = nameof(PushController.Egauge)
      }
    );
  }
}
