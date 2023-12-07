using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.Chart.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Contents;

namespace Mess.Chart.Controllers;

public class ChartController : Controller
{
  private readonly IAuthorizationService _authorizationService;

  private readonly IContentManager _contentManager;
  private readonly IServiceProvider _serviceProvider;

  public ChartController(
    IContentManager contentManager,
    IServiceProvider serviceProvider,
    IAuthorizationService authorizationService
  )
  {
    _contentManager = contentManager;
    _serviceProvider = serviceProvider;
    _authorizationService = authorizationService;
  }

  public async Task<IActionResult> Index(string contentItemId)
  {
    var metadataContentItem = await _contentManager.GetAsync(contentItemId);
    if (metadataContentItem is null) return NotFound();

    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        CommonPermissions.ViewContent,
        metadataContentItem
      )
    )
      return Forbid();

    var metadataPart = metadataContentItem.As<ChartPart>();
    if (metadataPart is null) return NotFound();

    var dataProvider = _serviceProvider
      .GetServices<IChartFactory>()
      .FirstOrDefault(
        dataProvider =>
          dataProvider.ContentType == metadataPart.ContentItem.ContentType
      );
    if (dataProvider is null)
      return StatusCode(500, "Chart data provider not found");

    var chartContentItem = await _contentManager.GetAsync(
      metadataPart.ChartContentItemId,
      VersionOptions.Latest
    );
    if (chartContentItem is null) return StatusCode(500, "Chart not found");

    var chart = await dataProvider.CreateChartAsync(
      metadataContentItem,
      chartContentItem
    );

    return chart is null
      ? StatusCode(500, "Chart creation failed")
      : Json(chart);
  }

  public async Task<IActionResult> Preview(string contentItemId)
  {
    var chartContentItem = await _contentManager.GetAsync(
      contentItemId,
      VersionOptions.Latest
    );
    if (chartContentItem is null) return NotFound();

    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        CommonPermissions.PreviewContent,
        chartContentItem
      )
    )
      return Forbid();

    var dataProvider = _serviceProvider
      .GetServices<IChartFactory>()
      .FirstOrDefault(
        dataProvider =>
          dataProvider.ContentType == PreviewChartFactory.ChartContentType
      );
    if (dataProvider is null)
      return StatusCode(500, "Preview chart data provider not found");

    var chart = await dataProvider.CreateChartAsync(
      new ContentItem(),
      chartContentItem
    );

    return chart is null
      ? StatusCode(500, "Chart creation failed")
      : Json(chart);
  }
}
