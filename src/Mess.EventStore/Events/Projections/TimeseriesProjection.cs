using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Mess.Timeseries.Client;
using Microsoft.Extensions.Logging;
using Mess.Util.OrchardCore.Tenants;

namespace Mess.EventStore.Events.Projections;

public class TimeseriesProjection : IProjection
{
  public ILogger? Logger { get; set; }
  public ITimeseriesClient? Client { get; set; }
  public ITenantProvider? Tenants { get; set; }

  public void Apply(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams
  )
  {
    if (Client is null)
    {
      throw new InvalidOperationException("Client is null");
    }

    if (Tenants is null)
    {
      throw new InvalidOperationException("Tenants is null");
    }

    if (Logger is null)
    {
      throw new InvalidOperationException("Logger is null");
    }

    var egaugeEvents = streams
      .SelectMany(stream => stream.Events)
      .Where(@event => @event.EventType == typeof(EgaugeMeasured))
      .OrderBy(@event => @event.Sequence)
      .Select(@event => @event.Data as EgaugeMeasured)
      .Where(@event => @event is not null)
      .Cast<EgaugeMeasured>();

    foreach (var egaugeEvent in egaugeEvents)
    {
      Client.AddMeasurement(
        new()
        {
          Tenant = Tenants.GetTenantName(),
          SourceId = "egauge",
          Timestamp = DateTime.UtcNow,
          Power = egaugeEvent.measurement.Power,
          Voltage = egaugeEvent.measurement.Voltage
        }
      );
    }

    Logger?.LogInformation("Applied {count} streams", streams.Count);
  }

  public async Task ApplyAsync(
    IDocumentOperations operations,
    IReadOnlyList<StreamAction> streams,
    CancellationToken cancellation
  )
  {
    if (Client is null)
    {
      throw new InvalidOperationException("Client is null");
    }

    if (Tenants is null)
    {
      throw new InvalidOperationException("Tenants is null");
    }

    if (Logger is null)
    {
      throw new InvalidOperationException("Logger is null");
    }

    var egaugeEvents = streams
      .SelectMany(stream => stream.Events)
      .Where(@event => @event.EventType == typeof(EgaugeMeasured))
      .OrderBy(@event => @event.Sequence)
      .Select(@event => @event.Data as EgaugeMeasured)
      .Where(@event => @event is not null)
      .Cast<EgaugeMeasured>();

    foreach (var egaugeEvent in egaugeEvents)
    {
      await Client.AddMeasurementAsync(
        new()
        {
          Tenant = Tenants.GetTenantName(),
          SourceId = "egauge",
          Timestamp = egaugeEvent.measurement.timestamp,
          Power = egaugeEvent.measurement.Power,
          Voltage = egaugeEvent.measurement.Voltage
        }
      );
    }

    Logger?.LogInformation("Applied {count} streams", streams.Count);
  }
}
