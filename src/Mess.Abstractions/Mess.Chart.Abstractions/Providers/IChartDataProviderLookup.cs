namespace Mess.Chart.Abstractions.Providers;

public interface IChartDataProviderLookup
{
  public IChartDataProvider? Get(string id);

  public bool Exists(string id);

  public IReadOnlyList<string> Ids { get; }
}
