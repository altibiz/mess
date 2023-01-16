using Mess.Chart.Abstractions.Models;

namespace Mess.Timeseries.Chart;

public record class TimeseriesChartProviderParameters(
  string Source,
  TimeSpan HistorySpan,
  List<TimeseriesChartProviderParametersField> Fields
) : IChartProviderParameters
{
  public string Provider => nameof(Mess.Timeseries);
}

public record class TimeseriesChartProviderParametersField(
  string Label,
  string Unit,
  string Color,
  string Field
);
