using Mess.Chart.Abstractions.Models;
using Mess.System.Extensions.Object;

namespace Mess.Chart.Abstractions.Extensions.System;

public static class ObjectExtensions
{
  public static TimeseriesChartDatapointModel ToTimeseriesChartDatapoint<T>(
    this T @object,
    string xField,
    string yField
  )
  {
    var x = @object.GetFieldOrPropertyValue<T, DateTime>(xField);
    var y = @object.GetFieldOrPropertyValue<T, float>(yField);

    return new TimeseriesChartDatapointModel(x, y);
  }
}
