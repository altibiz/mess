using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Mess.Chart.Controllers;

[Admin]
public class ChartAdminController : Controller
{
  public async Task<IActionResult> Create(
    string contentItemId,
    string contentType
  )
  {
    if (!await IsAuthorizedAsync())
    {
      return Forbid();
    }

    if (!IsValidChartContentType(contentType))
    {
      return NotFound();
    }

    var chart = await GetChartAsync(contentItemId);
    if (chart == null)
    {
      return NotFound();
    }

    var contentItem = await _contentManager.NewAsync(contentType);

    chart.Alter<ChartPart>(part => part.Chart = contentItem);
    await _contentManager.SaveDraftAsync(chart);

    var editor = await _contentItemDisplayManager.BuildEditorAsync(
      contentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );

    return View(editor);
  }

  [HttpPost]
  [ActionName("Create")]
  public async Task<IActionResult> CreatePost(
    string contentItemId,
    string contentType
  )
  {
    if (!await IsAuthorizedAsync())
    {
      return Forbid();
    }

    if (!IsValidChartContentType(contentType))
    {
      return NotFound();
    }

    var chart = await GetChartAsync(contentItemId);
    if (chart == null)
    {
      return NotFound();
    }

    var contentItem = await _contentManager.NewAsync(contentType);

    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      contentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );
    if (!ModelState.IsValid)
    {
      return View(model);
    }

    chart.Alter<ChartPart>(part => part.Chart = contentItem);
    await _contentManager.SaveDraftAsync(chart);

    return RedirectToAction(
      nameof(Edit),
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public async Task<IActionResult> Edit(string contentItemId)
  {
    if (!await IsAuthorizedAsync())
    {
      return Forbid();
    }

    var chart = await _contentManager.GetAsync(
      contentItemId,
      VersionOptions.Latest
    );
    if (chart is null)
    {
      return NotFound();
    }

    var contentItem = chart.As<ChartPart>().Chart;
    if (contentItem is null)
    {
      return NotFound();
    }

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      contentItem,
      _updateModelAccessor.ModelUpdater,
      false
    );

    return View(model);
  }

  [HttpPost]
  [ActionName("Edit")]
  public async Task<IActionResult> EditPost(string contentItemId)
  {
    if (!await IsAuthorizedAsync())
    {
      return Forbid();
    }

    var chart = await GetChartAsync(contentItemId);
    if (chart == null)
    {
      return NotFound();
    }

    var existing = chart.As<ChartPart>().Chart;
    if (existing == null)
    {
      return NotFound();
    }

    var contentItem = await _contentManager.NewAsync(existing.ContentType);
    contentItem.Merge(existing);

    dynamic model = await _contentItemDisplayManager.UpdateEditorAsync(
      contentItem,
      _updateModelAccessor.ModelUpdater,
      false
    );
    if (!ModelState.IsValid)
    {
      return View(model);
    }

    existing.Merge(
      contentItem.Content as object,
      new JsonMergeSettings
      {
        MergeArrayHandling = MergeArrayHandling.Replace,
        MergeNullValueHandling = MergeNullValueHandling.Merge
      }
    );
    chart.DisplayText = contentItem.DisplayText;
    await _contentManager.SaveDraftAsync(chart);

    return RedirectToAction(
      nameof(Edit),
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  public async Task<IActionResult> DeleteAsync(string contentItemId)
  {
    if (!await IsAuthorizedAsync())
    {
      return Forbid();
    }

    var chart = await GetChartAsync(contentItemId);
    if (chart == null)
    {
      return NotFound();
    }

    chart.Alter<ChartPart>(chart => chart.Chart = null);
    await _contentManager.SaveDraftAsync(chart);

    await _notifier.SuccessAsync(H["Chart deleted successfully."]);
    return RedirectToAction(
      nameof(Edit),
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId }
    );
  }

  private bool IsValidChartContentType(string contentType)
  {
    if (String.IsNullOrWhiteSpace(contentType))
    {
      return false;
    }

    var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(
      contentType
    );
    if (contentTypeDefinition is null)
    {
      return false;
    }

    var contentTypeSettings =
      contentTypeDefinition.GetSettings<ContentTypeSettings>();

    return contentTypeSettings is not null
      && !String.IsNullOrEmpty(contentTypeSettings.Stereotype)
      && contentTypeSettings.Stereotype.Equals("Chart");
  }

  private async Task<bool> IsAuthorizedAsync() =>
    await _authorizationService.AuthorizeAsync(
      User,
      ChartPermissions.ManageChart
    );

  private async Task<ContentItem> GetChartAsync(string contentItemId) =>
    _contentDefinitionManager.GetTypeDefinition("Chart").IsDraftable()
      ? await _contentManager.GetAsync(
        contentItemId,
        VersionOptions.DraftRequired
      )
      : await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

  public ChartAdminController(
    IContentManager contentManager,
    IContentDefinitionManager contentDefinitionManager,
    IAuthorizationService authorizationService,
    IContentItemDisplayManager contentItemDisplayManager,
    IUpdateModelAccessor updateModelAccessor,
    INotifier notifier,
    IHtmlLocalizer<ChartAdminController> localizer
  )
  {
    _contentManager = contentManager;
    _contentDefinitionManager = contentDefinitionManager;
    _authorizationService = authorizationService;
    _contentItemDisplayManager = contentItemDisplayManager;
    _updateModelAccessor = updateModelAccessor;
    _notifier = notifier;
    H = localizer;
  }

  private readonly IContentManager _contentManager;
  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly IContentItemDisplayManager _contentItemDisplayManager;
  private readonly IUpdateModelAccessor _updateModelAccessor;
  private readonly INotifier _notifier;
  private readonly IHtmlLocalizer H;
}
