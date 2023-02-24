using Mess.Chart.Abstractions.Models;

namespace Mess.Chart.Abstractions.Extensions.System;

public static class IEnumerableExtensions
{
  public static TimeseriesChartDatasetModel ToTimeseriesChartDataset<T>(
    this IEnumerable<T> objects,
    string label,
    string color,
    TimeSpan history,
    string xField,
    string yField
  ) =>
    new TimeseriesChartDatasetModel(
      Label: label,
      Color: color,
      History: history,
      Datapoints: objects
        .Select(@object => @object.ToTimeseriesChartDatapoint(xField, yField))
        .ToList()
    );
}