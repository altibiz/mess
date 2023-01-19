using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Mess.Chart.Controllers;

public class ChartController : Controller
{
  public async Task<IActionResult> Index(
    [FromQuery] ChartParameters parameters,
    [FromServices] IChartProviderLookup lookup
  )
  {
    var provider = lookup.Get(parameters.Provider);

    if (provider is null)
    {
      return BadRequest("Chart provider not found");
    }

    var specification = await provider.CreateChartAsync(parameters);

    if (specification is null)
    {
      return BadRequest("Chart specification failed");
    }

    return Json(specification);
  }
}
