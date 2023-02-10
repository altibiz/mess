using System.Security.Claims;
using Mess.Chart.Abstractions;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.OrchardCore.Extensions.OrchardCore;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;

namespace Mess.Chart.Services;

public class ChartService : IChartService
{
  public async Task<bool> IsAuthorizedAsync(ClaimsPrincipal user)
  {
    return await _authorizationService.AuthorizeAsync(
      user,
      ChartPermissions.ManageChart
    );
  }

  public async Task<ContentItem?> GetChartAsync(string contentItemId)
  {
    var contentItem = _contentDefinitionManager
      .GetTypeDefinition(ChartContentType)
      .IsDraftable()
      ? await _contentManager.GetAsync(
        contentItemId,
        VersionOptions.DraftRequired
      )
      : await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

    if (!contentItem.Has<ChartPart>())
    {
      return null;
    }

    return contentItem;
  }

  public async Task<ContentItem?> SaveChartAsync(ContentItem chart)
  {
    await _contentManager.SaveDraftAsync(chart);
    return chart;
  }

  public async Task<bool> IsValidConcreteChartTypeAsync(string contentType)
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

    return await Task.FromResult(
      contentTypeSettings is not null
        && !String.IsNullOrEmpty(contentTypeSettings.Stereotype)
        && contentTypeSettings.Stereotype.Equals(ConcreteChartStereotype)
    );
  }

  public async Task<ContentItem?> CreateConcreteChartAsync(
    ContentItem chart,
    string concreteChartContentType
  )
  {
    var concreteChart = await _contentManager.NewAsync(
      concreteChartContentType
    );
    if (concreteChart is null)
    {
      return null;
    }

    chart.Alter<ChartPart>(part => part.Chart = concreteChart);
    return concreteChart;
  }

  public Task<ContentItem?> ReadConcreteChartAsync(ContentItem chart)
  {
    return Task.FromResult(chart.As<ChartPart>()?.Chart);
  }

  public async Task<ContentItem?> UpdateConcreteChartAsync(
    ContentItem chart,
    ContentItem concreteChart
  )
  {
    var existingConcreteChart = await ReadConcreteChartAsync(chart);
    if (existingConcreteChart is null)
    {
      return null;
    }

    existingConcreteChart.Merge(concreteChart);

    chart.Alter<ChartPart>(part => part.Chart = existingConcreteChart);
    return await Task.FromResult(chart);
  }

  public async Task<ContentItem?> DeleteConcreteChartAsync(ContentItem chart)
  {
    chart.Alter<ChartPart>(part => part.Chart = null);
    return await Task.FromResult(chart);
  }

  public async Task<ContentItem?> CreateLineChartDatasetAsync(ContentItem chart)
  {
    if (!await IsLineChartAsync(chart))
    {
      return null;
    }

    var lineChartDataset = await _contentManager.NewAsync(
      LineChartDatasetContentType
    );
    if (lineChartDataset is null)
    {
      return null;
    }

    chart.Alter<ChartPart>(
      chartPart =>
        chartPart.Chart.Alter<LineChartPart>(lineChartPart =>
        {
          if (lineChartPart.Datasets is null)
          {
            lineChartPart.Datasets = new List<ContentItem>();
          }
          lineChartPart.Datasets.Add(lineChartDataset);
        })
    );

    return lineChartDataset;
  }

  public async Task<ContentItem?> ReadLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  )
  {
    return await Task.FromResult(
      chart
        .As<ChartPart>()
        ?.Chart?.As<LineChartPart>()
        ?.Datasets.FirstOrDefault(
          dataset => dataset.ContentItemId.Equals(lineChartDatasetContentItemId)
        )
    );
  }

  public async Task<ContentItem?> UpdateLineChartDatasetAsync(
    ContentItem chart,
    ContentItem lineChartDataset
  )
  {
    if (!await IsLineChartAsync(chart))
    {
      return null;
    }

    var existinglineChartDataset = await ReadLineChartDatasetAsync(
      chart,
      lineChartDataset.ContentItemId
    );
    if (existinglineChartDataset is null)
    {
      return null;
    }

    existinglineChartDataset.Merge(lineChartDataset);

    chart.Alter<ChartPart>(
      chartPart =>
        chartPart.Chart.Alter<LineChartPart>(lineChartPart =>
        {
          var index = lineChartPart.Datasets.FindIndex(
            currentLineChartDataset =>
              currentLineChartDataset.ContentItemId.Equals(
                existinglineChartDataset.ContentItemId
              )
          );

          lineChartPart.Datasets[index] = existinglineChartDataset;
        })
    );

    return lineChartDataset;
  }

  public async Task<ContentItem?> DeleteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  )
  {
    if (!await IsLineChartAsync(chart))
    {
      return null;
    }

    var existinglineChartDataset = await ReadLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (existinglineChartDataset is null)
    {
      return null;
    }

    chart.Alter<ChartPart>(
      chartPart =>
        chartPart.Chart.Alter<LineChartPart>(lineChartPart =>
        {
          var index = lineChartPart.Datasets.FindIndex(
            currentLineChartDataset =>
              currentLineChartDataset.ContentItemId.Equals(
                existinglineChartDataset.ContentItemId
              )
          );

          lineChartPart.Datasets.RemoveAt(index);
        })
    );

    return existinglineChartDataset;
  }

  public async Task<bool> IsValidLineChartDatasetContentTypeAsync(
    string contentType
  )
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

    return await Task.FromResult(
      contentTypeSettings is not null
        && !String.IsNullOrEmpty(contentTypeSettings.Stereotype)
        && contentTypeSettings.Stereotype.Equals(
          ConcreteLineChartDatasetStereotype
        )
    );
  }

  public async Task<ContentItem?> CreateConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId,
    string concreteLineChartDatasetContentType
  )
  {
    if (!await IsLineChartAsync(chart))
    {
      return null;
    }

    var lineChartDataset = await ReadLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (lineChartDataset is null)
    {
      return null;
    }

    var concreteLineChartDataset = await _contentManager.NewAsync(
      concreteLineChartDatasetContentType
    );
    if (concreteLineChartDataset is null)
    {
      return null;
    }

    lineChartDataset.Alter<LineChartDatasetPart>(
      lineChartDatasetPart =>
        lineChartDatasetPart.Dataset = concreteLineChartDataset
    );

    await UpdateLineChartDatasetAsync(chart, lineChartDataset);

    return concreteLineChartDataset;
  }

  public async Task<ContentItem?> ReadConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  )
  {
    return (
      await ReadLineChartDatasetAsync(chart, lineChartDatasetContentItemId)
    )
      ?.As<LineChartDatasetPart>()
      ?.Dataset;
  }

  public async Task<ContentItem?> UpdateConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId,
    ContentItem concreteLineChartDataset
  )
  {
    if (!await IsLineChartAsync(chart))
    {
      return null;
    }

    var lineChartDataset = await ReadLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (lineChartDataset is null)
    {
      return null;
    }

    var existingConcreteLineChartDataset = lineChartDataset
      .As<LineChartDatasetPart>()
      ?.Dataset;
    if (existingConcreteLineChartDataset is null)
    {
      return null;
    }

    lineChartDataset.Alter<LineChartDatasetPart>(
      lineChartDatasetPart =>
        lineChartDatasetPart.Dataset = existingConcreteLineChartDataset.Merge(
          concreteLineChartDataset
        )
    );

    await UpdateLineChartDatasetAsync(chart, lineChartDataset);

    return concreteLineChartDataset;
  }

  public async Task<ContentItem?> DeleteConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  )
  {
    if (!await IsLineChartAsync(chart))
    {
      return null;
    }

    var lineChartDataset = await ReadLineChartDatasetAsync(
      chart,
      lineChartDatasetContentItemId
    );
    if (lineChartDataset is null)
    {
      return null;
    }

    var concreteLineChartDataset = lineChartDataset
      .As<LineChartDatasetPart>()
      ?.Dataset;
    if (concreteLineChartDataset is null)
    {
      return null;
    }

    lineChartDataset.Alter<LineChartDatasetPart>(
      lineChartDatasetPart => lineChartDatasetPart.Dataset = null
    );

    await UpdateLineChartDatasetAsync(chart, lineChartDataset);

    return concreteLineChartDataset;
  }

  private async Task<bool> IsLineChartAsync(ContentItem chart)
  {
    return await Task.FromResult(
      chart.As<ChartPart>()?.Chart?.Has<LineChartPart>() is true
    );
  }

  private const string ChartContentType = "Chart";

  private const string ConcreteChartStereotype = "ConcreteChart";

  private const string LineChartDatasetContentType = "LineChartDataset";

  private const string ConcreteLineChartDatasetStereotype =
    "ConcreteLineChartDataset";

  public ChartService(
    IContentManager contentManager,
    IContentDefinitionManager contentDefinitionManager,
    IAuthorizationService authorizationService
  )
  {
    _contentManager = contentManager;
    _contentDefinitionManager = contentDefinitionManager;
    _authorizationService = authorizationService;
  }

  private readonly IContentManager _contentManager;
  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IAuthorizationService _authorizationService;
}
