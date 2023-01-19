namespace Mess.Chart.Abstractions.Models;

public record TimeseriesChartDatasetModel(
  string Label,
  string Color,
  IReadOnlyList<TimeseriesChartDatapointModel> Datapoints
) : LineChartDatasetModel("Timeseries", Label, Color);
