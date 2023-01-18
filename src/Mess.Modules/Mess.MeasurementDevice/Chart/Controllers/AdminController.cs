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
    string id,
    string chartContentItemId,
    string egaugeFieldItemId
  )
  {
    if (String.IsNullOrWhiteSpace(id))
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

    var contentItem = await _contentManager.NewAsync(id);

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      contentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );

    model.ChartContentItemId = chartContentItemId;
    model.EgaugeFieldItemId = egaugeFieldItemId;

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
