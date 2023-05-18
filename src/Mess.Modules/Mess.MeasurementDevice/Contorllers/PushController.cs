using Microsoft.AspNetCore.Mvc;
using Mess.System.Extensions.Strings;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using OrchardCore.Environment.Shell;
using Microsoft.Extensions.DependencyInjection;
using Mess.EventStore.Abstractions.Client;
using Mess.MeasurementDevice.EventStore;
using Mess.Tenants;

namespace Mess.MeasurementDevice.Controllers;

public class PushController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken] // TODO: security
  public async Task<IActionResult> Index(
    [FromServices] IShellFeaturesManager shellFeaturesManager,
    [FromServices] IServiceProvider services,
    [FromServices] ITenants tenants,
    string dispatcherId
  )
  {
    var dispatcher = services
      .GetServices<IMeasurementDispatcher>()
      .FirstOrDefault(dispatcher => dispatcher.Id == dispatcherId);
    if (dispatcher is null)
    {
      return BadRequest($"Unknown dispatcher");
    }

    var measurement = await Request.Body.EncodeAsync();

    var features = await shellFeaturesManager.GetEnabledFeaturesAsync();
    if (features.Any(feature => feature.Id == "EventStore"))
    {
      var client = services.GetRequiredService<IEventStoreClient>();
      await client.RecordEventsAsync<Measurements>(
        new Measured(
          tenants.Current.Name,
          DateTime.UtcNow,
          dispatcherId,
          measurement
        )
      );
    }
    else
    {
      dispatcher.Dispatch(measurement);
    }

    return Ok();
  }
}
