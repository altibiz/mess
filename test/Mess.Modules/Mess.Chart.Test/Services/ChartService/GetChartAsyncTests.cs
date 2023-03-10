using Mess.Chart.Abstractions.Models;
using Moq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record GetChartAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentManager> contentManager,
  Mock<IContentDefinitionManager> contentDefinitionManager
)
{
  [Fact]
  public async Task ReturnsNullWhenContentItemIsNull()
  {
    Setup((ContentItem?)null);

    var result = await chartService.GetChartAsync("123");

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsNullWhenContentItemHasNoChartPart()
  {
    var contentItem = new ContentItem();
    Setup(contentItem);

    var result = await chartService.GetChartAsync("123");

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsContentItemWhenContentItemHasChartPart()
  {
    var chartPart = new ChartPart();
    var contentItem = new ContentItem();
    contentItem.Weld(chartPart);
    Setup(contentItem);

    var result = await chartService.GetChartAsync("123");

    Assert.NotNull(result);
    Assert.Equal(contentItem, result);
  }

  private void Setup(ContentItem? contentItem)
  {
    contentManager
      .Setup(
        contentManager =>
          contentManager.GetAsync(
            It.IsAny<string>(),
            It.IsAny<VersionOptions>()
          )
      )
      .ReturnsAsync(contentItem);

    contentDefinitionManager
      .Setup(
        contentDefinitionManager =>
          contentDefinitionManager.GetTypeDefinition(It.IsAny<string>())
      )
      .Returns(new ContentTypeDefinition("Chart", "Chart"));
  }
}
