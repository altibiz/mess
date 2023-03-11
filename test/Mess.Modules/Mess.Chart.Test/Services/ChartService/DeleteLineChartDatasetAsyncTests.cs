using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record DeleteLineChartDatasetAsyncTests(
  Mess.Chart.Services.ChartService chartService
)
{
  public static object[][] DoesntDeleteDatasetWhenItIsNotPresentData = new[]
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
    typeof(DeleteLineChartDatasetAsyncTests),
    nameof(
      DeleteLineChartDatasetAsyncTests.DoesntDeleteDatasetWhenItIsNotPresentData
    )
  )]
  public async Task DoesntDeleteDatasetWhenItIsNotPresent(
    ContentItem chart,
    string contentItemId
  )
  {
    var result = await chartService.DeleteLineChartDatasetAsync(
      chart,
      contentItemId
    );

    Assert.Null(result);
  }

  public static object[][] DeletesTheDatasetWhenItIsPresentData = new[]
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
    typeof(DeleteLineChartDatasetAsyncTests),
    nameof(
      DeleteLineChartDatasetAsyncTests.DeletesTheDatasetWhenItIsPresentData
    )
  )]
  public async Task DeletesDatasetWhenItIsPresent(
    ContentItem chart,
    string contentItemId
  )
  {
    var result = await chartService.DeleteLineChartDatasetAsync(
      chart,
      contentItemId
    );

    Assert.NotNull(result);
  }
}
