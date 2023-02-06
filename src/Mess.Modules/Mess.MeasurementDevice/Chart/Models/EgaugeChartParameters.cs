using Mess.MeasurementDevice.Chart.Providers;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Chart.Models;

public class EgaugeChartParameters : IChartProviderParameters
{
  public string Provider => EgaugeChartProvider.ProviderId;

  public string Source { get; set; } = default!;
  public EgaugeChartHistory History { get; set; } = default!;
  public List<ContentItem> Fields { get; set; } = default!;
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
