namespace Mess.Chart.Abstractions.Models;

public record TimeseriesChartDatasetModel(
  string Label,
  string Color,
  TimeSpan History,
  IReadOnlyList<TimeseriesChartDatapointModel> Datapoints
) : LineChartDatasetModel(LineChartDatasetType.Timeseries, Label, Color);
