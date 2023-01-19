using Mess.Chart.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Mess.MeasurementDevice.Chart.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> AddEgaugeChartField(
    string chartContentItemId
  )
  {
    if (String.IsNullOrWhiteSpace(chartContentItemId))
    {
      return NotFound();
    }

    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return Forbid();
    }

    var contentItem = await _contentManager.NewAsync("EgaugeChartField");

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      contentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );

    model.ChartContentItemId = chartContentItemId;

    return View(model);
  }

  public AdminController(
    IContentManager contentManager,
    IAuthorizationService authorizationService,
    IContentItemDisplayManager contentItemDisplayManager,
    IUpdateModelAccessor updateModelAccessor
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _contentItemDisplayManager = contentItemDisplayManager;
    _updateModelAccessor = updateModelAccessor;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly IContentItemDisplayManager _contentItemDisplayManager;
  private readonly IUpdateModelAccessor _updateModelAccessor;
}
