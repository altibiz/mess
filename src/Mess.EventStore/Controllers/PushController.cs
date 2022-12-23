using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mess.EventStore.Controllers;

public class PushController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken]
  public IActionResult Egauge([FromServices] ILogger<PushController> logger)
  {
    logger.LogDebug(Request.Body.FromXmlStream()?.ToXml());

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    return Ok();
  }
}
