using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Controllers;

public class ChartController : Controller
{
  [HttpPost]
  public async Task<IActionResult> Index(
    [FromServices] IChartDataProviderLookup lookup,
    [FromBody] ContentItem contentItem
  )
  {
    var part = contentItem.As<ChartPart>();
    if (part is null)
    {
      return BadRequest("Chart part not present");
    }

    var dataProvider = lookup.Get(part.DataProviderId);
    if (dataProvider is null)
    {
      return BadRequest("Chart provider not found");
    }

    var chart = await dataProvider.CreateChartAsync(part.Chart);
    if (chart is null)
    {
      return BadRequest("Chart creation failed");
    }

    return Json(chart);
  }
}
