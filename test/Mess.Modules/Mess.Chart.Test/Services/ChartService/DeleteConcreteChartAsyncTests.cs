using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record DeleteConcreteChartAsyncTests(
  Mess.Chart.Services.ChartService chartService
)
{
  public static readonly object[][] DoesntDeleteConcreteChartWhenChartIsInvalidData =
    new[]
    {
      new[]
      {
        new object[] { new ContentItem() },
        new object[] { new ContentItem().Weld(new ChartPart()) },
        new object[]
        {
          new ContentItem().Weld(
            new ChartPart() { DataProviderId = "InvalidDataProviderId" }
          )
        },
        new object[]
        {
          new ContentItem().Weld(
            new ChartPart() { DataProviderId = Startup.TestDataProviderId }
          )
        },
      }
    };

  [Theory]
  [StaticData(
    typeof(DeleteConcreteChartAsyncTests),
    nameof(
      DeleteConcreteChartAsyncTests.DoesntDeleteConcreteChartWhenChartIsInvalidData
    )
  )]
  public async Task DoesntDeleteConcreteChartWhenChartIsInvalid(
    ContentItem chart
  )
  {
    var result = await chartService.DeleteConcreteChartAsync(chart);

    Assert.Null(result);
  }

  public static readonly object[][] DeletesConcreteChartWhenChartIsValidData =
    new[]
    {
      new[]
      {
        new object[]
        {
          new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
            new ChartPart()
            {
              DataProviderId = Startup.TestDataProviderId,
              Chart = new ContentItem().Weld(
                new LineChartPart()
                {
                  RootContentItemId = "TestContentItemId",
                  ChartDataProviderId = Startup.TestDataProviderId
                }
              )
            }
          )
        },
      }
    };

  [Theory]
  [StaticData(
    typeof(DeleteConcreteChartAsyncTests),
    nameof(
      DeleteConcreteChartAsyncTests.DeletesConcreteChartWhenChartIsValidData
    )
  )]
  public async Task DeletesConcreteChartWhenChartIsValid(ContentItem chart)
  {
    var result = await chartService.DeleteConcreteChartAsync(chart);

    Assert.NotNull(result);
    await Verify(result);
    Assert.Null(chart.As<ChartPart>().Chart);
    Assert.Equal(
      chart.As<ChartPart>().DataProviderId,
      result.As<LineChartPart>().ChartDataProviderId
    );
    Assert.Equal(
      chart.ContentItemId,
      result.As<LineChartPart>().RootContentItemId
    );
  }
}
