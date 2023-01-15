namespace Mess.Chart.Abstractions.Models;

public record class ChartParameters(
  string Provider,
  IChartProviderParameters ProviderParameters
);

public interface IChartProviderParameters
{
  public string Provider { get; }
}
