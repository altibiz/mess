namespace Mess.Chart.Abstractions.Providers;

public interface IChartProviderLookup
{
  public IChartProvider? Get(string id);

  public bool Exists(string id);

  public IReadOnlyList<string> Ids { get; }
}
