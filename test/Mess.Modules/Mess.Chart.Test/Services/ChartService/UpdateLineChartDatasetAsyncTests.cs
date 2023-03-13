using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore.Extensions.Objects;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record UpdateLineChartDatasetAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  ILogger<UpdateConcreteChartAsyncTests> logger
)
{
  public static object[][] DoesntUpdateDatasetWhenDatasetIsNotPresentData =
    new[]
    {
      new object[]
      {
        new ContentItem(),
        new ContentItem() { ContentItemId = "TestContentItemId" }
      },
      new object[]
      {
        new ContentItem().Weld(new ChartPart()),
        new ContentItem() { ContentItemId = "TestContentItemId" }
      },
      new object[]
      {
        new ContentItem().Weld(new ChartPart() { Chart = new ContentItem() }),
        new ContentItem() { ContentItemId = "TestContentItemId" }
      },
      new object[]
      {
        new ContentItem().Weld(
          new ChartPart()
          {
            Chart = new ContentItem().Weld(new LineChartPart())
          }
        ),
        new ContentItem() { ContentItemId = "TestContentItemId" }
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
        new ContentItem() { ContentItemId = "TestContentItemId" }
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
        new ContentItem() { ContentItemId = "TestContentItemId" }
      },
    };

  [Theory]
  [StaticData(
    typeof(UpdateLineChartDatasetAsyncTests),
    nameof(
      UpdateLineChartDatasetAsyncTests.DoesntUpdateDatasetWhenDatasetIsNotPresentData
    )
  )]
  public async Task DoesntUpdateDatasetWhenDatasetIsNotPresent(
    ContentItem chart,
    ContentItem lineChartDataset
  )
  {
    var result = await chartService.UpdateLineChartDatasetAsync(
      chart,
      lineChartDataset
    );

    Assert.Null(result);
  }

  private class SomeOtherPart : ContentPart
  {
    public string SomeOtherField { get; set; } = default!;
  }

  private class UpdatedPart : ContentPart
  {
    public string UpdatedField { get; set; } = default!;

    public string SomeOtherField { get; set; } = default!;
  }

  public static object[][] UpdatesDatasetWhenDatasetIsPresentData = new[]
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
                new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
                  new SomeOtherPart() { SomeOtherField = "SomeOther" }
                )
              }
            }
          )
        }
      ),
      new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
        new UpdatedPart() { UpdatedField = "Updated" }
      )
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
                new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
                  new SomeOtherPart() { SomeOtherField = "SomeOther" }
                )
              }
            }
          )
        }
      ),
      new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
        new UpdatedPart() { UpdatedField = "Updated" }
      )
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
                new ContentItem() { ContentItemId = "TestContentItemId" }
                  .Weld(
                    new UpdatedPart()
                    {
                      UpdatedField = "Previous",
                      SomeOtherField = "SomeOther"
                    }
                  )
                  .Weld(new SomeOtherPart() { SomeOtherField = "SomeOther" }),
              }
            }
          )
        }
      ),
      new ContentItem() { ContentItemId = "TestContentItemId" }.Weld(
        new UpdatedPart() { UpdatedField = "Updated" }
      )
    },
  };

  [Theory]
  [StaticData(
    typeof(UpdateLineChartDatasetAsyncTests),
    nameof(
      UpdateLineChartDatasetAsyncTests.UpdatesDatasetWhenDatasetIsPresentData
    )
  )]
  public async Task UpdatesDatasetWhenDatasetIsPresent(
    ContentItem chart,
    ContentItem lineChartDataset
  )
  {
    var result = await chartService.UpdateLineChartDatasetAsync(
      chart,
      lineChartDataset
    );

    Assert.NotNull(result);
    logger.LogInformation(result.ToNewtonsoftJson());
    var updatedPart = result.As<UpdatedPart>();
    Assert.NotNull(updatedPart);
    Assert.Equal("Updated", updatedPart.UpdatedField);
    var someOtherPart = result.As<SomeOtherPart>();
    Assert.NotNull(someOtherPart);
    Assert.Equal("SomeOther", someOtherPart.SomeOtherField);
  }
}
