using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.Chart.Controllers;

public class ChartController : Controller
{
  [HttpPost]
  public async Task<IActionResult> Index(
    [FromServices] IContentManager contentManager,
    [FromServices] IServiceProvider serviceProvider,
    string contentItemId
  )
  {
    var metadataContentItem = await contentManager.GetAsync(contentItemId);
    if (metadataContentItem is null)
    {
      return NotFound();
    }

    var metadataPart = metadataContentItem.As<ChartPart>();
    if (metadataPart is null)
    {
      return NotFound();
    }

    var dataProvider = serviceProvider
      .GetServices<IChartDataProvider>()
      .FirstOrDefault(
        dataProvider => dataProvider.Id == metadataPart.DataProviderId
      );
    if (dataProvider is null)
    {
      return StatusCode(500, "Chart data provider not found");
    }

    var chartContentItem = await contentManager.GetAsync(
      metadataPart.ChartContentItemId
    );
    if (chartContentItem is null)
    {
      return StatusCode(500, "Chart not found");
    }

    var chart = await dataProvider.CreateChartAsync(
      metadataContentItem,
      chartContentItem
    );
    if (chart is null)
    {
      return StatusCode(500, "Chart creation failed");
    }

    return Json(chart);
  }
}
