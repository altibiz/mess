using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Mess.Chart.Controllers;

[Admin]
public class ChartAdminController : Controller
{
  public async Task<IActionResult> Create(string contentType)
  {
    if (String.IsNullOrWhiteSpace(contentType))
    {
      return NotFound();
    }

    var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(
      contentType
    );
    if (contentTypeDefinition is null)
    {
      return NotFound();
    }

    var contentTypeSettings =
      contentTypeDefinition.GetSettings<ContentTypeSettings>();
    if (
      contentTypeSettings is null
      || String.IsNullOrEmpty(contentTypeSettings.Stereotype)
      || !contentTypeSettings.Stereotype.Equals("Chart")
    )
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

  public IActionResult Edit()
  {
    throw new NotImplementedException();
  }

  public IActionResult Delete()
  {
    throw new NotImplementedException();
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
