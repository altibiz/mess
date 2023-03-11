using Mess.Chart.Abstractions.Models;
using Moq;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record CreateConcreteChartAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentManager> contentManager
)
{
  [Fact]
  public async Task ReturnsNullWhenContentTypeIsEmpty()
  {
    Setup("TestContentType");

    var chart = new ContentItem();

    var result = await chartService.CreateConcreteChartAsync(
      chart,
      string.Empty
    );

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsNullWhenContentTypeIsInvalid()
  {
    Setup("TestContentType");

    var chart = new ContentItem();

    var result = await chartService.CreateConcreteChartAsync(
      chart,
      "InvalidContentType"
    );

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsBoundContentItemWhenContentTypeIsValid()
  {
    Setup("TestContentType");

    var chart = new ContentItem();
    chart.ContentItemId = "TestContentItemId";
    chart.Weld(new ChartPart() { DataProviderId = "TestDataProviderId" });

    var result = await chartService.CreateConcreteChartAsync(
      chart,
      "TestContentType"
    );

    Assert.NotNull(result);
    var nestedChartPart = result.Get<NestedChartPart>("TestContentTypePart");
    Assert.NotNull(nestedChartPart);
    Assert.Equal("TestContentItemId", nestedChartPart.RootContentItemId);
    Assert.Equal("TestDataProviderId", nestedChartPart.ChartDataProviderId);
    var chartPart = chart.As<ChartPart>();
    Assert.NotNull(chartPart);
    Assert.Equal(result, chartPart.Chart);
  }

  private void Setup(string contentType)
  {
    contentManager
      .Setup(contentManager => contentManager.NewAsync(contentType))
      .ReturnsAsync(new ContentItem());
  }
}
