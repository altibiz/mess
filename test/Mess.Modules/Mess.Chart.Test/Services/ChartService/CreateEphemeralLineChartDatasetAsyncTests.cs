using Mess.Chart.Abstractions.Models;
using Moq;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record CreateEphemeralLineChartDatasetAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentManager> contentManager
)
{
  public static object[][] DoesntCreateDatasetWhenChartIsNotLineChartData =
    new[]
    {
      new[] { new ContentItem() },
      new[] { new ContentItem().Weld(new ChartPart()) },
      new[]
      {
        new ContentItem().Weld(new ChartPart() { Chart = new ContentItem() })
      },
    };

  [Theory]
  [StaticData(
    typeof(CreateEphemeralLineChartDatasetAsyncTests),
    nameof(
      CreateEphemeralLineChartDatasetAsyncTests.DoesntCreateDatasetWhenChartIsNotLineChartData
    )
  )]
  public async Task DoesntCreateDatasetWhenChartIsNotLineChart(
    ContentItem chart
  )
  {
    var result = await chartService.CreateEphemeralLineChartDatasetAsync(chart);

    Assert.Null(result);
  }

  [Fact]
  public async Task CreatesDatasetWhenChartIsLineChart()
  {
    Setup(new ContentItem());

    var chart = new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
      new ChartPart()
      {
        DataProviderId = "TestDataProviderId",
        Chart = new ContentItem().Weld(new LineChartPart())
      }
    );

    var result = await chartService.CreateEphemeralLineChartDatasetAsync(chart);

    Assert.NotNull(result);
    var nestedChartPart = result.Get<NestedChartPart>("LineChartDatasetPart");
    Assert.NotNull(nestedChartPart);
    Assert.Equal("TestContentItemId", nestedChartPart.RootContentItemId);
    Assert.Equal("TestDataProviderId", nestedChartPart.ChartDataProviderId);
  }

  private void Setup(ContentItem contentItem)
  {
    contentManager
      .Setup(contentManager => contentManager.NewAsync("LineChartDataset"))
      .ReturnsAsync(contentItem);
  }
}
