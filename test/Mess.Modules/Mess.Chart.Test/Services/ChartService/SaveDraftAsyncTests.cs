using Moq;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record SaveDraftAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentManager> contentManager
)
{
  [Fact]
  public async Task CallsSaveDraftAsync()
  {
    var contentItem = new ContentItem();

    contentManager
      .Setup(
        contentManager => contentManager.SaveDraftAsync(It.IsAny<ContentItem>())
      )
      .Returns(Task.CompletedTask);

    await chartService.SaveChartAsync(contentItem);

    Assert.Equal(
      nameof(IContentManager.SaveDraftAsync),
      contentManager.Invocations[0].Method.Name
    );
    Assert.Equal(contentItem, contentManager.Invocations[0].Arguments[0]);
  }
}
