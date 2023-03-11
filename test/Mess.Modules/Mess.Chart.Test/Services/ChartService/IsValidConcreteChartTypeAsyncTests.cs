using Moq;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record IsValidConcreteChartTypeAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IContentDefinitionManager> contentDefinitionManager
)
{
  [Fact]
  public async Task ReturnsFalseWhenContentTypeIsEmpty()
  {
    var result = await chartService.IsValidConcreteChartTypeAsync(string.Empty);
    Assert.False(result);
  }

  [Fact]
  public async Task ReturnsFalseWhenContentTypeIsInvalid()
  {
    Setup("TestContentType", null);
    var result = await chartService.IsValidConcreteChartTypeAsync(
      "InvalidContentType"
    );
    Assert.False(result);
  }

  [Fact]
  public async Task ReturnsFalseWhenContentTypeHasNoStereotype()
  {
    Setup("TestContentType", null);
    var result = await chartService.IsValidConcreteChartTypeAsync(
      "TestContentType"
    );
    Assert.False(result);
  }

  [Fact]
  public async Task ReturnsFalseWhenContentTypeHasWrongStereotype()
  {
    Setup("TestContentType", "WrongStereotype");
    var result = await chartService.IsValidConcreteChartTypeAsync(
      "TestContentType"
    );
    Assert.False(result);
  }

  [Fact]
  public async Task ReturnsTrueWhenContentTypeHasRightStereotype()
  {
    Setup("TestContentType", "ConcreteChart");
    var result = await chartService.IsValidConcreteChartTypeAsync(
      "TestContentType"
    );
    Assert.False(result);
  }

  private void Setup(string contentType, string? stereotype)
  {
    var contentTypeDefinition = new ContentTypeDefinition(
      contentType,
      contentType
    );
    contentTypeDefinition.PopulateSettings<ContentTypeSettings>(
      new ContentTypeSettings { Stereotype = stereotype }
    );

    contentDefinitionManager
      .Setup(
        contentDefinitionManager =>
          contentDefinitionManager.GetTypeDefinition(contentType)
      )
      .Returns(contentTypeDefinition);
  }
}
