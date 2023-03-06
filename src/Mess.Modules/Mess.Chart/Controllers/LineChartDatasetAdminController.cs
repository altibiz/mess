using Mess.Chart.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Mess.Chart.Controllers;

[Admin]
public class LineChartDatasetAdminController : Controller
{
  public async Task<IActionResult> Create(string contentItemId)
  {
    if (!await _chartService.IsAuthorizedAsync(User))
    {
      return Forbid();
    }

    var chart = await _chartService.GetChartAsync(contentItemId);
    if (chart is null)
    {
      return NotFound();
    }

    var lineChartDataset =
      await _chartService.CreateEphemeralLineChartDatasetAsync(chart);
    if (lineChartDataset is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      lineChartDataset,
      _updateModelAccessor.ModelUpdater,
      true
    );

    model.ContentItemId = contentItemId;
    model.LineChartDatasetContentItemId = lineChartDataset.ContentItemId;

    return View(model);
  }

  [HttpPost]
  [ActionName("Create")]
  public async Task<IActionResult> CreatePost(string contentItemId)
  {
    if (!await _chartService.IsAuthorizedAsync(User))
    {
      return Forbid();
    }

    var chart = await _chartService.GetChartAsync(contentItemId);
    if (chart is null)
    {
      return NotFound();
    }

    var lineChartDataset = await _chartService.CreateLineChartDatasetAsync(
      chart
    );
    if (lineChartDataset is null)
    {
      await _notifier.ErrorAsync(H["Failed creating line chart dataset."]);
      return RedirectToAction(
        "Edit",
        "Admin",
        new { area = "OrchardCore.Contents", contentItemId }
      );
    }

    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      lineChartDataset,
      _updateModelAccessor.ModelUpdater,
      false
    );
    if (!ModelState.IsValid)
    {
      model.ContentItemId = contentItemId;
      model.LineChartDatasetContentItemId = lineChartDataset.ContentItemId;

      return View(model);
    }

    var updatedLineChartDataset =
      await _chartService.UpdateLineChartDatasetAsync(chart, lineChartDataset);
    if (updatedLineChartDataset is null)
    {
      await _notifier.ErrorAsync(H["Failed creating line chart dataset."]);
    }
    else
    {
      await _chartService.SaveChartAsync(chart);

      await _notifier.SuccessAsync(
        H["Line chart dataset created successfully."]
      );
    }

    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public async Task<IActionResult> Edit(
    string contentItemId,
    string lineChartDatasetContentItemId
  )
  {
    if (!await _chartService.IsAuthorizedAsync(User))
    {
      return Forbid();
    }

    var chart = await _chartService.GetChartAsync(contentItemId);
    if (chart is null)
    {
      return NotFound();
    }

    var lineChartDataaset = await _chartService.ReadLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (lineChartDataaset is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      lineChartDataaset,
      _updateModelAccessor.ModelUpdater,
      false
    );

    model.ContentItemId = contentItemId;
    model.LineChartDatasetContentItemId = lineChartDatasetContentItemId;

    return View(model);
  }

  [HttpPost]
  [ActionName("Edit")]
  public async Task<IActionResult> EditPost(
    string contentItemId,
    string lineChartDatasetContentItemId
  )
  {
    if (!await _chartService.IsAuthorizedAsync(User))
    {
      return Forbid();
    }

    var chart = await _chartService.GetChartAsync(contentItemId);
    if (chart is null)
    {
      return NotFound();
    }

    var lineChartDataaset = await _chartService.ReadLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (lineChartDataaset is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      lineChartDataaset,
      _updateModelAccessor.ModelUpdater,
      false
    );
    if (!ModelState.IsValid)
    {
      model.ContentItemId = contentItemId;
      model.LineChartDatasetContentItemId = lineChartDatasetContentItemId;

      return View(model);
    }

    var updatedLineChartDataset =
      await _chartService.UpdateLineChartDatasetAsync(chart, lineChartDataaset);
    if (updatedLineChartDataset is null)
    {
      await _notifier.ErrorAsync(H["Failed updating line chart dataset."]);

      return RedirectToAction(
        "Edit",
        "Admin",
        new { area = "OrchardCore.Contents", contentItemId }
      );
    }

    await _chartService.SaveChartAsync(chart);

    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  [HttpPost]
  public async Task<IActionResult> Delete(
    string contentItemId,
    string lineChartDatasetContentItemId
  )
  {
    if (!await _chartService.IsAuthorizedAsync(User))
    {
      return Forbid();
    }

    var chart = await _chartService.GetChartAsync(contentItemId);
    if (chart is null)
    {
      return NotFound();
    }

    var lineChartDataset = await _chartService.DeleteLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (lineChartDataset is null)
    {
      await _notifier.ErrorAsync(H["Failed deleting line chart dataset."]);
    }
    else
    {
      await _chartService.SaveChartAsync(chart);

      await _notifier.SuccessAsync(
        H["Line chart dataset deleted successfully."]
      );
    }

    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public LineChartDatasetAdminController(
    IChartService chartService,
    IUpdateModelAccessor updateModelAccessor,
    IContentManager contentManager,
    IContentItemDisplayManager contentItemDisplayManager,
    INotifier notifier,
    IHtmlLocalizer<LineChartDatasetAdminController> localizer
  )
  {
    _chartService = chartService;
    _updateModelAccessor = updateModelAccessor;
    _contentManager = contentManager;
    _contentItemDisplayManager = contentItemDisplayManager;
    _notifier = notifier;
    H = localizer;
  }

  private readonly IChartService _chartService;
  private readonly IUpdateModelAccessor _updateModelAccessor;
  private readonly IContentManager _contentManager;
  private readonly IContentItemDisplayManager _contentItemDisplayManager;
  private readonly INotifier _notifier;
  private readonly IHtmlLocalizer H;
}
