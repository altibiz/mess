using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Mess.Chart.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> CreateLineChartDataset(string contentType)
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
      || !contentTypeSettings.Stereotype.Equals("LineChartDataset")
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

    var lineChartDatasetContentItem = await _contentManager.NewAsync(
      "LineChartDataset"
    );
    var lineChartDatasetPart =
      lineChartDatasetContentItem.As<LineChartDatasetPart>();

    var chartTypeContentItem = await _contentManager.NewAsync(contentType);
    lineChartDatasetPart.Dataset = chartTypeContentItem;

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      chartTypeContentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );

    return View(model);
  }

  public async Task<IActionResult> CreateChart(string contentType)
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

    dynamic model = await _contentItemDisplayManager.BuildEditorAsync(
      chartContentItem,
      _updateModelAccessor.ModelUpdater,
      true
    );

    return View(model);
  }

  private JObject? FindChart(JObject contentItem, string chartContentItemId)
  {
    if (contentItem["ContentItemId"]?.Value<string>() == chartContentItemId)
    {
      return contentItem;
    }

    var chartPart = contentItem.GetValue("ChartPart");
    if (chartPart is not null)
    {
      var chart = chartPart["Chart"];
      if (chart is not null)
      {
        if (chart["ContentItemId"]?.Value<string>() == chartContentItemId)
        {
          return contentItem;
        }
      }
    }

    // TODO: handle flows?
    var flowPart = contentItem.GetValue("FlowPart");
    if (flowPart is null)
    {
      return null;
    }

    return null;
  }

  public AdminController(
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
