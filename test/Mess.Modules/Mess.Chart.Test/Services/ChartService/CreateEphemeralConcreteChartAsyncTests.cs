using Moq;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record CreateEphemeralConcreteChartAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentManager> contentManager,
  ISnapshotFixture snapshotFixture
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
    nameof(DoesntCreateConcreteChartWhenContentTypeIsInvalidData)
  )]
  public async Task DoesntCreateConcreteChartWhenContentTypeIsInvalid(
    string setupContentType,
    string contentType
  )
  {
    var verificationHash = await snapshotFixture.MakeVerificationHash(
      setupContentType,
      contentType
    );

    Setup(setupContentType);

    var chart = new ContentItem();

    var result = await chartService.CreateConcreteChartAsync(
      chart,
      contentType
    );

    result.Should().BeNull();

    await snapshotFixture.Verify(new { result, chart }, verificationHash);
  }

  [Theory]
  [NewtonsoftJsonAssetData]
  public async Task DoesntCreateConcreteChartWhenChartIsInvalid(
    ContentItem chart
  )
  {
    var verificationHash = await snapshotFixture.MakeVerificationHash(chart);

    Setup("TestContentType");

    var result = await chartService.CreateConcreteChartAsync(
      chart,
      "TestContentType"
    );

    result.Should().BeNull();

    await snapshotFixture.Verify(new { result, chart }, verificationHash);
  }

  [Theory]
  [NewtonsoftJsonAssetData]
  public async Task CreatesConcreteChartWhenChartIsValid(ContentItem chart)
  {
    var verificationHash = await snapshotFixture.MakeVerificationHash(chart);

    Setup("TestContentType");

    var result = await chartService.CreateConcreteChartAsync(
      chart,
      "TestContentType"
    );

    result.Should().NotBeNull();

    await snapshotFixture.Verify(new { result, chart }, verificationHash);
  }

  private void Setup(string contentType)
  {
    contentManager
      .Setup(contentManager => contentManager.NewAsync(contentType))
      .ReturnsAsync(new ContentItem());
  }
}
