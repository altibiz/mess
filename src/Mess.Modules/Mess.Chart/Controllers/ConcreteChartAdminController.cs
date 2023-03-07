using Mess.Chart.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Mess.Chart.Controllers;

[Admin]
public class ConcreteChartAdminController : Controller
{
  public async Task<IActionResult> Create(
    string contentItemId,
    string contentType
  )
  {
    if (!await _chartService.IsAuthorizedAsync(User))
    {
      return Forbid();
    }

    if (!await _chartService.IsValidConcreteChartTypeAsync(contentType))
    {
      return NotFound();
    }

    var chart = await _chartService.GetChartAsync(contentItemId);
    if (chart is null)
    {
      return NotFound();
    }

    var concreteChart = await _chartService.CreateConcreteChartAsync(
      chart,
      contentType
    );
    if (concreteChart is null)
    {
      await _notifier.ErrorAsync(H["Failed creating concrete chart."]);
    }
    else
    {
      await _chartService.SaveChartAsync(chart);

      await _notifier.SuccessAsync(H["Concrete chart created successfully."]);
    }

    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public ConcreteChartAdminController(
    IChartService chartService,
    IUpdateModelAccessor updateModelAccessor,
    IContentItemDisplayManager contentItemDisplayManager,
    INotifier notifier,
    IHtmlLocalizer<ConcreteChartAdminController> localizer
  )
  {
    _chartService = chartService;
    _updateModelAccessor = updateModelAccessor;
    _contentItemDisplayManager = contentItemDisplayManager;
    _notifier = notifier;
    H = localizer;
  }

  private readonly IChartService _chartService;
  private readonly IUpdateModelAccessor _updateModelAccessor;
  private readonly IContentItemDisplayManager _contentItemDisplayManager;
  private readonly INotifier _notifier;
  private readonly IHtmlLocalizer H;
}
