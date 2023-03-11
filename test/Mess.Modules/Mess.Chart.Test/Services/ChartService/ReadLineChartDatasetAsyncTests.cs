using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record ReadLineChartDatasetAsyncTests(
  Mess.Chart.Services.ChartService chartService
)
{
  public static object[][] DoesntReadDatasetWhenDatasetIsNotPresentData = new[]
  {
    new object[] { new ContentItem(), "TestContentItemId" },
    new object[]
    {
      new ContentItem().Weld(new ChartPart()),
      "TestContentItemId"
    },
    new object[]
    {
      new ContentItem().Weld(new ChartPart() { Chart = new ContentItem() }),
      "TestContentItemId"
    },
    new object[]
    {
      new ContentItem().Weld(
        new ChartPart() { Chart = new ContentItem().Weld(new LineChartPart()) }
      ),
      "TestContentItemId"
    },
    new object[]
    {
      new ContentItem().Weld(
        new ChartPart()
        {
          Chart = new ContentItem().Weld(
            new LineChartPart() { Datasets = new List<ContentItem>() }
          )
        }
      ),
      "TestContentItemId"
    },
    new object[]
    {
      new ContentItem().Weld(
        new ChartPart()
        {
          Chart = new ContentItem().Weld(
            new LineChartPart()
            {
              Datasets = new List<ContentItem>()
              {
                new ContentItem() { ContentItemId = "OtherTestContentItemId" }
              }
            }
          )
        }
      ),
      "TestContentItemId"
    },
  };

  [Theory]
  [StaticData(
    typeof(ReadLineChartDatasetAsyncTests),
    nameof(
      ReadLineChartDatasetAsyncTests.DoesntReadDatasetWhenDatasetIsNotPresentData
    )
  )]
  public async Task DoesntReadDatasetWhenDatasetIsNotPresent(
    ContentItem chart,
    string contentItemId
  )
  {
    var result = await chartService.ReadLineChartDatasetAsync(
      chart,
      contentItemId
    );

    Assert.Null(result);
  }

  public static object[][] ReadsDatasetWhenDatasetIsPresentData = new[]
  {
    new object[]
    {
      new ContentItem().Weld(
        new ChartPart()
        {
          Chart = new ContentItem().Weld(
            new LineChartPart()
            {
              Datasets = new List<ContentItem>()
              {
                new ContentItem() { ContentItemId = "TestContentItemId" }
              }
            }
          )
        }
      ),
      "TestContentItemId"
    },
    new object[]
    {
      new ContentItem().Weld(
        new ChartPart()
        {
          Chart = new ContentItem().Weld(
            new LineChartPart()
            {
              Datasets = new List<ContentItem>()
              {
                new ContentItem() { ContentItemId = "OtherContentItemId" },
                new ContentItem() { ContentItemId = "TestContentItemId" },
              }
            }
          )
        }
      ),
      "TestContentItemId"
    },
  };

  [Theory]
  [StaticData(
    typeof(ReadLineChartDatasetAsyncTests),
    nameof(ReadLineChartDatasetAsyncTests.ReadsDatasetWhenDatasetIsPresentData)
  )]
  public async Task ReadsDatasetWhenDatasetIsPresent(
    ContentItem chart,
    string contentItemId
  )
  {
    var result = await chartService.ReadLineChartDatasetAsync(
      chart,
      contentItemId
    );

    Assert.NotNull(result);
    Assert.Equal(contentItemId, result.ContentItemId);
  }
}
