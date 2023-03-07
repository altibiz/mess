using Mess.Chart.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Mess.Chart.Controllers;

[Admin]
public class ConcreteLineChartDatasetAdminController : Controller
{
  public async Task<IActionResult> Create(
    string contentItemId,
    string lineChartDatasetContentItemId,
    string contentType
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

    var concreteLineChartDataset =
      await _chartService.CreateEphemeralConcreteLineChartDatasetAsync(
        chart,
        lineChartDatasetContentItemId,
        contentType
      );
    if (concreteLineChartDataset is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      concreteLineChartDataset,
      _updateModelAccessor.ModelUpdater,
      true
    );

    model.ContentItemId = contentItemId;
    model.LineChartDatasetContentItemId = lineChartDatasetContentItemId;
    model.ContentType = contentType;

    return View(model);
  }

  [HttpPost]
  [ActionName("Create")]
  public async Task<IActionResult> CreatePost(
    string contentItemId,
    string lineChartDatasetContentItemId,
    string contentType
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

    var concreteLineChartDataset =
      await _chartService.CreateConcreteLineChartDatasetAsync(
        chart,
        lineChartDatasetContentItemId,
        contentType
      );
    if (concreteLineChartDataset is null)
    {
      await _notifier.ErrorAsync(
        H["Failed creating concrete line chart dataset."]
      );
      return RedirectToAction(
        "Edit",
        "Admin",
        new { area = "OrchardCore.Contents", contentItemId }
      );
    }

    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      concreteLineChartDataset,
      _updateModelAccessor.ModelUpdater,
      false
    );
    if (!ModelState.IsValid)
    {
      model.ContentItemId = contentItemId;
      model.LineChartDatasetContentItemId = lineChartDatasetContentItemId;
      model.ContentType = contentType;

      return View(model);
    }

    var updatedConcreteLineChartDataset =
      await _chartService.UpdateConcreteLineChartDatasetAsync(
        chart,
        lineChartDatasetContentItemId,
        concreteLineChartDataset
      );
    if (updatedConcreteLineChartDataset is null)
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
    string lineChartDatasetContentItemId,
    string contentType
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

    var concreteLineChartDataset =
      await _chartService.ReadConcreteLineChartDatasetAsync(
        chart,
        lineChartDatasetContentItemId
      );
    if (concreteLineChartDataset is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      concreteLineChartDataset,
      _updateModelAccessor.ModelUpdater,
      false
    );

    model.ContentItemId = contentItemId;
    model.LineChartDatasetContentItemId = lineChartDatasetContentItemId;
    model.ContentType = concreteLineChartDataset.ContentType;

    return View(model);
  }

  [HttpPost]
  [ActionName("Edit")]
  public async Task<IActionResult> EditPost(
    string contentItemId,
    string lineChartDatasetContentItemId,
    string contentType
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

    var concreteLineChartDataset =
      await _chartService.ReadConcreteLineChartDatasetAsync(
        chart,
        lineChartDatasetContentItemId
      );
    if (concreteLineChartDataset is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      concreteLineChartDataset,
      _updateModelAccessor.ModelUpdater,
      false
    );
    if (!ModelState.IsValid)
    {
      model.ContentItemId = contentItemId;
      model.LineChartDatasetContentItemId = lineChartDatasetContentItemId;
      model.ContentType = concreteLineChartDataset.ContentType;

      return View(model);
    }

    var updatedLineChartDataset =
      await _chartService.UpdateConcreteLineChartDatasetAsync(
        chart,
        lineChartDatasetContentItemId,
        concreteLineChartDataset
      );
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

  public ConcreteLineChartDatasetAdminController(
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
