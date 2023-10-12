using Microsoft.AspNetCore.Mvc;

namespace Mess.Ozds.Controllers;

public class BillingController : Controller
{
  public async Task<IActionResult> List()
  {
    return Ok();
  }
}
