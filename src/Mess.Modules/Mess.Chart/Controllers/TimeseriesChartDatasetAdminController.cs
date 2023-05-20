using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Mess.Chart.Controllers;

[Admin]
public class TimeseriesChartDatasetAdminController : Controller
{
  public async Task<IActionResult> Create(
    string contentItemId,
    string? chartContentItemId
  )
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return Forbid();
    }

    TimeseriesChartItem? chart = null;
    DashboardItem? dashboard = null;
    if (chartContentItemId is null)
    {
      chart = await _contentManager.GetContentAsync<TimeseriesChartItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }
    else
    {
      dashboard = await _contentManager.GetContentAsync<DashboardItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
      if (dashboard is not null)
      {
        chart = dashboard.FlowPart.Value.Widgets
          .FirstOrDefault(chart => chart.ContentItemId == chartContentItemId)
          ?.AsContent<TimeseriesChartItem>();
      }
    }
    if (chart is null)
    {
      return NotFound();
    }

    var timeseriesChartDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    if (timeseriesChartDataset is null)
    {
      return StatusCode(500);
    }

    timeseriesChartDataset.Inner.Content.ChartDataProviderId = chart
      .TimeseriesChartPart
      .Value
      .ChartDataProviderId;
    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      timeseriesChartDataset,
      _updateModelAccessor.ModelUpdater,
      true
    );

    model.ContentItemId = contentItemId;
    model.ChartContentItemId = chartContentItemId;
    model.DataProviderId = chart.TimeseriesChartPart.Value.ChartDataProviderId;

    return View(model);
  }

  [HttpPost]
  [ActionName("Create")]
  public async Task<IActionResult> CreatePost(
    string contentItemId,
    string? chartContentItemId
  )
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return Forbid();
    }

    TimeseriesChartItem? chart = null;
    DashboardItem? dashboard = null;
    if (chartContentItemId is null)
    {
      chart = await _contentManager.GetContentAsync<TimeseriesChartItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }
    else
    {
      dashboard = await _contentManager.GetContentAsync<DashboardItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
      if (dashboard is not null)
      {
        chart = dashboard.FlowPart.Value.Widgets
          .FirstOrDefault(chart => chart.ContentItemId == chartContentItemId)
          ?.AsContent<TimeseriesChartItem>();
      }
    }
    if (chart is null)
    {
      return NotFound();
    }
    var timeseriesChartDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    if (timeseriesChartDataset is null)
    {
      return StatusCode(500);
    }

    timeseriesChartDataset.Inner.Content.ChartDataProviderId = chart
      .TimeseriesChartPart
      .Value
      .ChartDataProviderId;
    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      timeseriesChartDataset,
      _updateModelAccessor.ModelUpdater,
      true
    );
    if (!ModelState.IsValid)
    {
      model.ContentItemId = contentItemId;
      model.ChartContentItemId = chartContentItemId;
      model.DataProviderId = chart
        .TimeseriesChartPart
        .Value
        .ChartDataProviderId;

      return View(model);
    }

    chart.Alter(
      chart => chart.TimeseriesChartPart,
      timeseriesChart => timeseriesChart.Datasets.Add(timeseriesChartDataset)
    );
    if (dashboard is not null)
    {
      dashboard.Alter(
        dashboard => dashboard.FlowPart,
        flow =>
          flow.Widgets = flow.Widgets
            .Select(
              widget =>
                widget.ContentItemId == chartContentItemId ? chart : widget
            )
            .ToList()
      );
      await _contentManager.SaveDraftAsync(dashboard);
    }
    else
    {
      await _contentManager.SaveDraftAsync(chart);
    }

    await _notifier.SuccessAsync(
      H["Timeseries chart dataset created successfully."]
    );
    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public async Task<IActionResult> Edit(
    string contentItemId,
    string datasetContentItemId,
    string? chartContentItemId
  )
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return Forbid();
    }

    TimeseriesChartItem? chart = null;
    DashboardItem? dashboard = null;
    if (chartContentItemId is null)
    {
      chart = await _contentManager.GetContentAsync<TimeseriesChartItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }
    else
    {
      dashboard = await _contentManager.GetContentAsync<DashboardItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
      if (dashboard is not null)
      {
        chart = dashboard.FlowPart.Value.Widgets
          .FirstOrDefault(chart => chart.ContentItemId == chartContentItemId)
          ?.AsContent<TimeseriesChartItem>();
      }
    }
    if (chart is null)
    {
      return NotFound();
    }
    var timeseriesChartDataset =
      chart.TimeseriesChartPart.Value.Datasets.FirstOrDefault(
        dataset => dataset.ContentItemId == datasetContentItemId
      );
    if (timeseriesChartDataset is null)
    {
      return NotFound();
    }

    timeseriesChartDataset.Content.ChartDataProviderId = chart
      .TimeseriesChartPart
      .Value
      .ChartDataProviderId;
    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      timeseriesChartDataset,
      _updateModelAccessor.ModelUpdater,
      false
    );
    model.ContentItemId = contentItemId;
    model.ChartContentItemId = chartContentItemId;
    model.DatasetContentItemId = datasetContentItemId;
    model.DataProviderId = chart.TimeseriesChartPart.Value.ChartDataProviderId;
    return View(model);
  }

  [HttpPost]
  [ActionName("Edit")]
  public async Task<IActionResult> EditPost(
    string contentItemId,
    string datasetContentItemId,
    string? chartContentItemId
  )
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return Forbid();
    }

    TimeseriesChartItem? chart = null;
    DashboardItem? dashboard = null;
    if (chartContentItemId is null)
    {
      chart = await _contentManager.GetContentAsync<TimeseriesChartItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }
    else
    {
      dashboard = await _contentManager.GetContentAsync<DashboardItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
      if (dashboard is not null)
      {
        chart = dashboard.FlowPart.Value.Widgets
          .FirstOrDefault(chart => chart.ContentItemId == chartContentItemId)
          ?.AsContent<TimeseriesChartItem>();
      }
    }
    if (chart is null)
    {
      return NotFound();
    }
    var timeseriesChartDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    if (timeseriesChartDataset is null)
    {
      return StatusCode(500);
    }

    timeseriesChartDataset.Inner.Content.ChartDataProviderId = chart
      .TimeseriesChartPart
      .Value
      .ChartDataProviderId;
    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      timeseriesChartDataset,
      _updateModelAccessor.ModelUpdater,
      true
    );
    if (!ModelState.IsValid)
    {
      model.ContentItemId = contentItemId;
      model.ChartContentItemId = chartContentItemId;
      model.DatasetContentItemId = datasetContentItemId;
      model.DataProviderId = chart
        .TimeseriesChartPart
        .Value
        .ChartDataProviderId;

      return View(model);
    }

    chart.Alter(
      chart => chart.TimeseriesChartPart,
      timeseriesChart =>
        timeseriesChart.Datasets = timeseriesChart.Datasets
          .Select(
            dataset =>
              dataset.ContentItemId == datasetContentItemId
                ? timeseriesChartDataset
                : dataset
          )
          .ToList()
    );
    if (dashboard is not null)
    {
      dashboard.Alter(
        dashboard => dashboard.FlowPart,
        flow =>
          flow.Widgets = flow.Widgets
            .Select(
              widget =>
                widget.ContentItemId == chartContentItemId ? chart : widget
            )
            .ToList()
      );
      await _contentManager.SaveDraftAsync(dashboard);
    }
    else
    {
      await _contentManager.SaveDraftAsync(chart);
    }

    await _notifier.SuccessAsync(
      H["Timeseries chart dataset edited successfully."]
    );
    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  [HttpPost]
  public async Task<IActionResult> Delete(
    string contentItemId,
    string datasetContentItemId,
    string? chartContentItemId
  )
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return Forbid();
    }

    TimeseriesChartItem? chart = null;
    DashboardItem? dashboard = null;
    if (chartContentItemId is null)
    {
      chart = await _contentManager.GetContentAsync<TimeseriesChartItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }
    else
    {
      dashboard = await _contentManager.GetContentAsync<DashboardItem>(
        contentItemId,
        VersionOptions.DraftRequired
      );
      if (dashboard is not null)
      {
        chart = dashboard.FlowPart.Value.Widgets
          .FirstOrDefault(chart => chart.ContentItemId == chartContentItemId)
          ?.AsContent<TimeseriesChartItem>();
      }
    }
    if (chart is null)
    {
      return NotFound();
    }
    var timeseriesChartDataset =
      chart.TimeseriesChartPart.Value.Datasets.FirstOrDefault(
        dataset => dataset.ContentItemId == datasetContentItemId
      );
    if (timeseriesChartDataset is null)
    {
      return NotFound();
    }

    chart.Alter(
      chart => chart.TimeseriesChartPart,
      timeseriesChart =>
        timeseriesChart.Datasets = timeseriesChart.Datasets
          .Where(dataset => dataset.ContentItemId != datasetContentItemId)
          .ToList()
    );
    if (dashboard is not null)
    {
      dashboard.Alter(
        dashboard => dashboard.FlowPart,
        flow =>
          flow.Widgets = flow.Widgets
            .Select(
              widget =>
                widget.ContentItemId == chartContentItemId ? chart : widget
            )
            .ToList()
      );
      await _contentManager.SaveDraftAsync(dashboard);
    }
    else
    {
      await _contentManager.SaveDraftAsync(chart);
    }

    await _notifier.SuccessAsync(
      H["Timeseries chart dataset deleted successfully."]
    );
    return RedirectToAction(
      "Edit",
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public TimeseriesChartDatasetAdminController(
    IUpdateModelAccessor updateModelAccessor,
    IContentManager contentManager,
    IContentItemDisplayManager contentItemDisplayManager,
    INotifier notifier,
    IAuthorizationService authorizationService,
    IHtmlLocalizer<TimeseriesChartDatasetAdminController> localizer
  )
  {
    _updateModelAccessor = updateModelAccessor;
    _contentManager = contentManager;
    _contentItemDisplayManager = contentItemDisplayManager;
    _notifier = notifier;
    _authorizationService = authorizationService;
    H = localizer;
  }

  private readonly IUpdateModelAccessor _updateModelAccessor;
  private readonly IContentManager _contentManager;
  private readonly IContentItemDisplayManager _contentItemDisplayManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly INotifier _notifier;
  private readonly IHtmlLocalizer H;
}
