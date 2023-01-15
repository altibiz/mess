using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Controllers;

public class ChartController : Controller
{
  public async Task<IActionResult> Index([FromQuery] ChartParameters parameters)
  {
    var provider = _lookup.Get(parameters.Provider);

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

  public ChartController(
    IContentManager contentManager,
    IChartProviderLookup lookup
  )
  {
    _contentManager = contentManager;
    _lookup = lookup;
  }

  private readonly IContentManager _contentManager;
  private readonly IChartProviderLookup _lookup;
}
