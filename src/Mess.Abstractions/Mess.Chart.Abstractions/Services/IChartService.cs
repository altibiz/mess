using System.Security.Claims;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Services;

// TODO: use strongly typed content item where possible
public interface IChartService
{
  public Task<bool> IsAuthorizedAsync(ClaimsPrincipal user);

  public Task<ContentItem?> GetChartAsync(string contentItemId);

  public Task<ContentItem?> SaveChartAsync(ContentItem chart);

  // ConcreteChart

  public Task<bool> IsValidConcreteChartTypeAsync(string contentType);

  public Task<ContentItem?> CreateEphemeralConcreteChartAsync(
    ContentItem chart,
    string concreteChartContentType
  );

  public Task<ContentItem?> CreateConcreteChartAsync(
    ContentItem chart,
    string concreteChartContentType
  );

  public Task<ContentItem?> ReadConcreteChartAsync(ContentItem chart);

  public Task<ContentItem?> UpdateConcreteChartAsync(
    ContentItem chart,
    ContentItem concreteChart
  );

  public Task<ContentItem?> DeleteConcreteChartAsync(ContentItem chart);

  // LineChartDataset

  public Task<ContentItem?> CreateEphemeralLineChartDatasetAsync(
    ContentItem lineChart
  );

  public Task<ContentItem?> CreateLineChartDatasetAsync(ContentItem lineChart);

  public Task<ContentItem?> ReadLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  );

  public Task<ContentItem?> UpdateLineChartDatasetAsync(
    ContentItem chart,
    ContentItem lineChartDataset
  );

  public Task<ContentItem?> DeleteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  );

  // ConcreteLineChartDataset

  public Task<bool> IsValidLineChartDatasetContentTypeAsync(string contentType);

  public Task<ContentItem?> CreateEphemeralConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId,
    string concreteLineChartDatasetContentType
  );

  public Task<ContentItem?> CreateConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId,
    string concreteLineChartDatasetContentType
  );

  public Task<ContentItem?> ReadConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  );

  public Task<ContentItem?> UpdateConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId,
    ContentItem concreteLineChartDataset
  );

  public Task<ContentItem?> DeleteConcreteLineChartDatasetAsync(
    ContentItem chart,
    string lineChartDatasetContentItemId
  );
}
