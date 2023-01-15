namespace Mess.Chart.Abstractions.Providers;

public interface IChartProviderLookup
{
  public IChartProvider? Get(string id);
}
