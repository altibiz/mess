using Microsoft.AspNetCore.Mvc;
using Mess.Timeseries.ViewModels;
using OrchardCore.Admin;

namespace Mess.Timeseries.Controllers;

[Admin]
public class AdminController : Controller
{
  public IActionResult Index()
  {
    return View(new TimeseriesViewModel());
  }
}
