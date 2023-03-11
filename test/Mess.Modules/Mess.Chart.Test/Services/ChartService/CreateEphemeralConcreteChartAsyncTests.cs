using Mess.Chart.Abstractions.Models;
using Moq;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record CreateEphemeralConcreteChartAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentManager> contentManager
)
{
  public static readonly object[][] DoesntCreateConcreteChartWhenContentTypeIsInvalidData =
    new[]
    {
      new object[] { "TestContentType", string.Empty },
      new object[] { "TestContentType", "InvalidContentType" },
    };

  [Theory]
  [StaticData(
    typeof(CreateEphemeralConcreteChartAsyncTests),
    nameof(
      CreateEphemeralConcreteChartAsyncTests.DoesntCreateConcreteChartWhenContentTypeIsInvalidData
    )
  )]
  public async Task DoesntCreateConcreteChartWhenContentTypeIsInvalid(
    string setupContentType,
    string contentType
  )
  {
    Setup(setupContentType);

    var chart = new ContentItem();

    var result = await chartService.CreateEphemeralConcreteChartAsync(
      new ContentItem(),
      contentType
    );

    Assert.Null(result);
  }

  public static readonly object[][] DoesntCreateConcreteChartWhenChartIsInvalidData =
    new[]
    {
      new object[] { new ContentItem() },
      new object[] { new ContentItem().Weld(new ChartPart()) },
    };

  [Theory]
  [StaticData(
    typeof(CreateEphemeralConcreteChartAsyncTests),
    nameof(
      CreateEphemeralConcreteChartAsyncTests.DoesntCreateConcreteChartWhenChartIsInvalidData
    )
  )]
  public async Task DoesntCreateConcreteChartWhenChartIsInvalid(
    ContentItem contentItem
  )
  {
    Setup("TestContentType");

    var result = await chartService.CreateEphemeralConcreteChartAsync(
      contentItem,
      "TestContentType"
    );

    Assert.Null(result);
  }

  public static readonly object[][] CreatesConcreteChartWhenChartIsValidData =
    new[]
    {
      new object[]
      {
        new ContentItem().Weld(
          new ChartPart() { DataProviderId = "TestDataProviderId" }
        )
      },
      new object[]
      {
        new ContentItem().Weld(
          new ChartPart()
          {
            DataProviderId = "TestDataProviderId",
            Chart = new ContentItem()
          }
        )
      },
    };

  [Theory]
  [StaticData(
    typeof(CreateEphemeralConcreteChartAsyncTests),
    nameof(
      CreateEphemeralConcreteChartAsyncTests.CreatesConcreteChartWhenChartIsValidData
    )
  )]
  public async Task CreatesConcreteChartWhenChartIsValid(ContentItem chart)
  {
    Setup("TestContentType");

    var result = await chartService.CreateEphemeralConcreteChartAsync(
      chart,
      "TestContentType"
    );

    Assert.NotNull(result);
    var nestedChartPart = result.Get<NestedChartPart>("TestContentTypePart");
    Assert.NotNull(nestedChartPart);
    Assert.Equal("TestContentItemId", nestedChartPart.RootContentItemId);
    Assert.Equal("TestDataProviderId", nestedChartPart.ChartDataProviderId);
    var chartPart = chart.As<ChartPart>();
    Assert.NotEqual(chartPart.Chart, result);
  }

  private void Setup(string contentType)
  {
    contentManager
      .Setup(contentManager => contentManager.NewAsync(contentType))
      .ReturnsAsync(new ContentItem());
  }
}
