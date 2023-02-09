using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Mess.Chart.Controllers;

[Admin]
public class ChartAdminController : Controller
{
  public async Task<IActionResult> Create(string contentType)
  {
    if (!await IsAuthorizedAsync())
    {
      return Forbid();
    }

    if (!IsValidChartContentType(contentType))
    {
      return NotFound();
    }

    var chartContentItem = await _contentManager.NewAsync("Chart");
    var chartPart = chartContentItem.As<ChartPart>();

    var chartTypeContentItem = await _contentManager.NewAsync(contentType);
    chartPart.Chart = chartTypeContentItem;

    var editor = await _contentItemDisplayManager.BuildEditorAsync(
      chartContentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );

    return View(editor);
  }

  [HttpPost]
  [ActionName("Create")]
  public async Task<IActionResult> CreatePost(
    string contentType,
    string contentItemId
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

    ContentItem chart;

    var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(
      "Chart"
    );

    if (!contentTypeDefinition.IsDraftable())
    {
      chart = await _contentManager.GetAsync(
        contentItemId,
        VersionOptions.Latest
      );
    }
    else
    {
      chart = await _contentManager.GetAsync(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }

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

    ContentItem chart;

    var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(
      "Chart"
    );

    if (!contentTypeDefinition.IsDraftable())
    {
      chart = await _contentManager.GetAsync(
        contentItemId,
        VersionOptions.Latest
      );
    }
    else
    {
      chart = await _contentManager.GetAsync(
        contentItemId,
        VersionOptions.DraftRequired
      );
    }

    if (chart == null)
    {
      return NotFound();
    }

    var existing = chart.As<ChartPart>().Chart;
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
      contentItem.Content,
      new JsonMergeSettings
      {
        MergeArrayHandling = MergeArrayHandling.Replace,
        MergeNullValueHandling = MergeNullValueHandling.Merge
      }
    );

    // Merge doesn't copy the properties
    menuItem[nameof(ContentItem.DisplayText)] = contentItem.DisplayText;

    await _contentManager.SaveDraftAsync(chart);

    return RedirectToAction(
      nameof(Edit),
      "Admin",
      new { area = "OrchardCore.Contents", contentItemId = menuContentItemId }
    );
  }

  public IActionResult Delete()
  {
    throw new NotImplementedException();
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
    if (
      contentTypeSettings is null
      || String.IsNullOrEmpty(contentTypeSettings.Stereotype)
      || !contentTypeSettings.Stereotype.Equals("Chart")
    )
    {
      return false;
    }

    return true;
  }

  private async Task<bool> IsAuthorizedAsync()
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        ChartPermissions.ManageChart
      )
    )
    {
      return false;
    }

    return true;
  }

  public ChartAdminController(
    IContentManager contentManager,
    IContentDefinitionManager contentDefinitionManager,
    IAuthorizationService authorizationService,
    IContentItemDisplayManager contentItemDisplayManager,
    IUpdateModelAccessor updateModelAccessor
  )
  {
    _contentManager = contentManager;
    _contentDefinitionManager = contentDefinitionManager;
    _authorizationService = authorizationService;
    _contentItemDisplayManager = contentItemDisplayManager;
    _updateModelAccessor = updateModelAccessor;
  }

  private readonly IContentManager _contentManager;
  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly IContentItemDisplayManager _contentItemDisplayManager;
  private readonly IUpdateModelAccessor _updateModelAccessor;
}
