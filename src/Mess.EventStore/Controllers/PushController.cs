using Microsoft.AspNetCore.Mvc;
using Mess.EventStore.Parsers.Egauge;
using Mess.EventStore.Events.Streams;
using Mess.EventStore.Events;
using Mess.EventStore.Client;
using Mess.EventStore.ViewModels;

namespace Mess.EventStore.Controllers;

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

    await store.RecordEventsAsync<EgaugeMeasurementStream>(
      new EgaugeMeasured(measurement.Value)
    );

    return Ok();
  }

  [HttpGet]
  public async Task<IActionResult> Display(
    [FromServices] IEventStoreClient store
  )
  {
    var lastEvent = await store.LastEventAsync<EgaugeMeasured>();
    if (lastEvent is null)
    {
      return StatusCode(500, "No events");
    }

    return View(new EgaugeDisplayViewModel(lastEvent.timestamp));
  }
}
