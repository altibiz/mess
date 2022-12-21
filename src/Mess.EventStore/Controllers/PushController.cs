using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Mess.EventStore.Controllers;

public class PushController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Egauge(
    // NOTE: this doesn't work for some reason
    // [FromBody] XDocument yes,
    [FromServices]
      ILogger<PushController> logger
  )
  {
    using (var reader = new StreamReader(Request.Body))
    {
      var xml = await reader.ReadToEndAsync();
      logger.LogDebug(xml);
      try
      {
        var document = XDocument.Parse(xml);
        logger.LogDebug(document.ToString());
      }
      catch
      {
        logger.LogDebug("Failed parsing xml");
      }
    }

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    return Ok();
  }
}
