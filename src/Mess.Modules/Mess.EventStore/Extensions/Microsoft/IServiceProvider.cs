using Marten;
using Mess.EventStore.Client;
using Mess.EventStore.Events.Projections;
using Mess.Timeseries.Client;
using Mess.Util.OrchardCore.Tenants;
using Mess.EventStore.Extensions.Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mess.EventStore.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static void UseEventStore(this IServiceProvider services)
  {
    var client = services.GetRequiredService<IEventStoreClient>();
    var connected = client.CheckConnection();
    if (!connected)
    {
      throw new InvalidOperationException("EventStore client not connected");
    }
  }

  public static void UseTimeseriesProjection(this IServiceProvider services)
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
  }
}
