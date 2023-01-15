using Mess.Chart.Abstractions.Models;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartProvider
{
  public string Id { get; }

  public ChartSpecification? CreateChart(ChartParameters parameters);
  public Task<ChartSpecification?> CreateChartAsync(ChartParameters parameters);
}
