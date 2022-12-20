using Microsoft.AspNetCore.Mvc;

namespace Mess.EventStore.Controllers;

public class PushController : Controller
{
  public IActionResult Index()
  {
    return Ok(new { connected = true });
  }
}
