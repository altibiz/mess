using Marten.Events;
using Mess.EventStore.Abstractions.Events;
using Mess.MeasurementDevice.Events;
using Mess.Timeseries.Abstractions.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.Services;

public class MeasurementProjectionDispatcher : IProjectionDispatcher
{
  public void Apply(
    IServiceProvider services,
    IReadOnlyList<StreamAction> streams
  )
  {
    var timeseriesClient = services.GetRequiredService<ITimeseriesClient>();
    var logger = services.GetRequiredService<
      ILogger<MeasurementProjectionDispatcher>
    >();

    var egaugeEvents = streams
      .SelectMany(stream => stream.Events)
      .Where(@event => @event.EventType == typeof(EgaugeMeasured))
      .OrderBy(@event => @event.Sequence)
      .Select(
        @event => (Data: @event.Data as EgaugeMeasured, Tenant: @event.TenantId)
      )
      .Where(@event => @event.Data is not null)
      .Cast<(EgaugeMeasured Data, string Tenant)>();

    foreach (var egaugeEvent in egaugeEvents)
    {
      timeseriesClient.AddMeasurement(
        new(
          tenant: egaugeEvent.Tenant,
          sourceId: "egauge",
          timestamp: egaugeEvent.Data.measurement.timestamp,
          power: egaugeEvent.Data.measurement.Power,
          voltage: egaugeEvent.Data.measurement.Voltage
        )
      );
    }

    logger.LogInformation("Applied {count} streams", streams.Count);
  }

  public async Task ApplyAsync(
    IServiceProvider services,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellationToken
  )
  {
    var timeseriesClient = services.GetRequiredService<ITimeseriesClient>();
    var logger = services.GetRequiredService<
      ILogger<MeasurementProjectionDispatcher>
    >();

    var egaugeEvents = streams
      .SelectMany(stream => stream.Events)
      .Where(@event => @event.EventType == typeof(EgaugeMeasured))
      .OrderBy(@event => @event.Sequence)
      .Select(
        @event => (Data: @event.Data as EgaugeMeasured, Tenant: @event.TenantId)
      )
      .Where(@event => @event.Data is not null)
      .Cast<(EgaugeMeasured Data, string Tenant)>();

    foreach (var egaugeEvent in egaugeEvents)
    {
      await timeseriesClient.AddMeasurementAsync(
        new(
          tenant: egaugeEvent.Tenant,
          sourceId: "egauge",
          timestamp: egaugeEvent.Data.measurement.timestamp,
          power: egaugeEvent.Data.measurement.Power,
          voltage: egaugeEvent.Data.measurement.Voltage
        )
      );

      if (cancellationToken.IsCancellationRequested)
      {
        logger.LogInformation(
          "Applied {count} streams and cancelled",
          streams.Count
        );
        return;
      }
    }

    logger.LogInformation("Applied {count} streams", streams.Count);
  }
}
