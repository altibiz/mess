using Mess.Chart.Abstractions.Models;
using Mess.MeasurementDevice.Chart.Providers;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Chart.Models;

public record class EgaugeChartParameters(
  string Source,
  EgaugeChartHistory History,
  List<ContentItem> Fields
) : IChartProviderParameters
{
  public string Provider => EgaugeChartProvider.ProviderId;
}

public enum EgaugeChartHistory
{
  Hour,
  Day,
  Week,
  Month
};

public static class EgaugeChartHistoryExtensions
{
  public static TimeSpan ToTimeSpan(this EgaugeChartHistory history) =>
    history switch
    {
      EgaugeChartHistory.Hour => TimeSpan.FromHours(1),
      EgaugeChartHistory.Day => TimeSpan.FromDays(1),
      EgaugeChartHistory.Week => TimeSpan.FromDays(7),
      EgaugeChartHistory.Month => TimeSpan.FromDays(30),
      _ => TimeSpan.FromHours(1),
    };
}
