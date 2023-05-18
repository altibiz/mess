using Mess.EventStore.Abstractions.Events;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.EventStore;

public class MeasurementProjectionDispatcher : IProjectionDispatcher
{
  public void Apply(IServiceProvider services, IEvents events)
  {
    var client = services.GetRequiredService<IMeasurementClient>();
    var logger = services.GetRequiredService<
      ILogger<MeasurementProjectionDispatcher>
    >();

    foreach (var measured in events.OfType<Measured>())
    {
      var dispatcher = services
        .GetServices<IMeasurementDispatcher>()
        .FirstOrDefault(dispatcher => dispatcher.Id == measured.DispatcherId);
      if (dispatcher is null)
      {
        continue;
      }

      dispatcher.Dispatch(measured.Measurement);
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

    foreach (var measured in events.OfType<Measured>())
    {
      var dispatcher = services
        .GetServices<IMeasurementDispatcher>()
        .FirstOrDefault(dispatcher => dispatcher.Id == measured.DispatcherId);
      if (dispatcher is null)
      {
        continue;
      }

      await dispatcher.DispatchAsync(measured.Measurement);

      if (cancellationToken.IsCancellationRequested)
      {
        logger.LogInformation("Applied events and cancelled");
        return;
      }
    }

    logger.LogInformation("Applied events");
  }
}
