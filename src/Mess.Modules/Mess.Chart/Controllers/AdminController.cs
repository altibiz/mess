using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;

namespace Mess.Chart.Controllers;

[Admin]
public class AdminController : Controller
{
  public IActionResult Index()
  {
    return Ok();
  }
}
