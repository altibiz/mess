using Microsoft.AspNetCore.Mvc;
using Mess.MeasurementDevice.Abstractions.Parsers.Egauge;
using Mess.EventStore.Abstractions.Client;
using Mess.System.Extensions.Object;
using Mess.MeasurementDevice.Events;

namespace Mess.MeasurementDevice.Controllers;

public class PushController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Egauge(
    [FromServices] IEgaugeParser parser,
    [FromServices] IEventStoreClient store
  )
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var xdocument = await Request.Body.FromXmlStreamAsync();
    if (xdocument is null)
    {
      return BadRequest("Xml is invalid");
    }

    var measurement = parser.Parse(xdocument);
    if (measurement is null)
    {
      return BadRequest("Measurement is invalid");
    }

    await store.RecordEventsAsync<MeasurementStream>(
      new EgaugeMeasured(measurement.Value)
    );

    return Ok();
  }
}
