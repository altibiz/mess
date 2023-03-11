using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record ReadConcreteChartAsyncTests(
  Mess.Chart.Services.ChartService chartService
)
{
  [Fact]
  public async Task ReturnsNullWhenContentItemHasNoChartPart()
  {
    var chart = new ContentItem();

    var result = await chartService.ReadConcreteChartAsync(chart);

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsNullWhenChartPartHasNoConcreteChart()
  {
    var chart = new ContentItem();
    chart.Weld(new ChartPart());

    var result = await chartService.ReadConcreteChartAsync(chart);

    Assert.Null(result);
  }

  [Fact]
  public async Task ReturnsContentItemWhenConcreteChartExists()
  {
    var concreteChart = new ContentItem();

    var chart = new ContentItem();
    chart.Weld(new ChartPart() { Chart = concreteChart });

    var result = await chartService.ReadConcreteChartAsync(chart);

    Assert.Equal(concreteChart, result);
  }
}
