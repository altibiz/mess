using Mess.Chart.Abstractions.Models;

namespace Mess.Chart.Abstractions.Extensions.System;

public static class IEnumerableExtensions
{
  public static LineChartDataset ToLineChartDataset<T>(
    this IEnumerable<T> objects,
    string label,
    string? unit,
    string color,
    string xField,
    string yField
  ) =>
    new LineChartDataset(
      Label: label,
      Unit: unit,
      Color: color,
      Data: objects
        .Select(@object => @object.ToLineChartData(xField, yField))
        .ToList()
    );
}
