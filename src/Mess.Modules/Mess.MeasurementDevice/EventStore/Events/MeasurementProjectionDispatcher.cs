using Mess.EventStore.Abstractions.Events;
using Mess.MeasurementDevice.Abstractions.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.EventStore.Events;

public class MeasurementProjectionDispatcher : IProjectionDispatcher
{
  public void Apply(IServiceProvider services, IEvents events)
  {
    var client = services.GetRequiredService<IMeasurementClient>();
    var logger = services.GetRequiredService<
      ILogger<MeasurementProjectionDispatcher>
    >();

    foreach (var egaugeEvent in events.OfType<EgaugeMeasured>())
    {
      client.AddEgaugeMeasurement(
        new(
          Tenant: egaugeEvent.Tenant,
          Timestamp: egaugeEvent.Timestamp,
          Source: egaugeEvent.Source,
          Power: egaugeEvent.Model.Power,
          Voltage: egaugeEvent.Model.Voltage
        )
      );
    }

    logger.LogInformation("Applied events");
  }

  public async Task ApplyAsync(
    IServiceProvider services,
    IEvents events,
    CancellationToken cancellationToken
  )
  {
    var client = services.GetRequiredService<IMeasurementClient>();
    var logger = services.GetRequiredService<
      ILogger<MeasurementProjectionDispatcher>
    >();

    foreach (var egaugeEvent in events.OfType<EgaugeMeasured>())
    {
      await client.AddEgaugeMeasurementAsync(
        new(
          Tenant: egaugeEvent.Tenant,
          Timestamp: egaugeEvent.Timestamp,
          Source: egaugeEvent.Source,
          Power: egaugeEvent.Model.Power,
          Voltage: egaugeEvent.Model.Voltage
        )
      );

      if (cancellationToken.IsCancellationRequested)
      {
        logger.LogInformation("Applied events and cancelled");
        return;
      }
    }

    logger.LogInformation("Applied events");
  }
}
