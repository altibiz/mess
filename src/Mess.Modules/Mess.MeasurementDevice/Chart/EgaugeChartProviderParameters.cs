using Mess.Chart.Abstractions.Models;

namespace Mess.MeasurementDevice.Chart;

public record class EgaugeChartProviderParameters(
  string Source,
  TimeSpan HistorySpan,
  List<EgaugeChartProviderParametersField> Fields
) : IChartProviderParameters
{
  public string Provider => EgaugeChartProvider.ProviderId;
}

public record class EgaugeChartProviderParametersField(
  string Field,
  string Label,
  string Unit,
  string Color
);
