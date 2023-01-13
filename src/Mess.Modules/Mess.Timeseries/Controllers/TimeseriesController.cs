using Microsoft.AspNetCore.Mvc;
using Mess.Timeseries.ViewModels;

namespace Mess.Timeseries.Controllers;

public class TimeseriesController : Controller
{
  [HttpGet]
  public IActionResult Index()
  {
    return View(new TimeseriesViewModel { });
  }
}
