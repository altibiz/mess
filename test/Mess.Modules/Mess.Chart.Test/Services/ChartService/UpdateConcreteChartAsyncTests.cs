using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record UpdateConcreteChartAsyncTests(
  Mess.Chart.Services.ChartService chartService
)
{
  [Fact]
  public async Task ReturnsNullWhenContentItemHasNoChartPart()
  {
    var updatedConcreteChart = new ContentItem();
    updatedConcreteChart.Weld(
      new LineChartPart { Datasets = new List<ContentItem>() }
    );

    var chart = new ContentItem();

    var result = await chartService.UpdateConcreteChartAsync(
      chart,
      updatedConcreteChart
    );

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsNullWhenChartPartHasNoConcreteChart()
  {
    var updatedConcreteChart = new ContentItem();
    updatedConcreteChart.Weld(
      new LineChartPart { Datasets = new List<ContentItem>() }
    );

    var chart = new ContentItem();
    chart.Weld(new ChartPart());

    var result = await chartService.UpdateConcreteChartAsync(
      chart,
      updatedConcreteChart
    );

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsContentItemWhenConcreteChartExistsAndMergesProperly()
  {
    var updatedConcreteChart = new ContentItem();
    updatedConcreteChart.Weld(
      new LineChartPart { Datasets = new List<ContentItem>() }
    );

    var concreteChart = new ContentItem();

    var chart = new ContentItem();
    chart.Weld(new ChartPart() { Chart = concreteChart });

    var result = await chartService.UpdateConcreteChartAsync(
      chart,
      updatedConcreteChart
    );

    Assert.Equal(concreteChart, result);
    var lineChartPart = result.As<LineChartPart>();
    Assert.NotNull(lineChartPart);
    Assert.NotNull(lineChartPart.Datasets);
  }
}
