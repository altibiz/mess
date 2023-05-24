namespace Mess.Chart.Abstractions.Descriptors;

public record TimeseriesChartDatasetDescriptor(
  string Label,
  string Color,
  IReadOnlyList<TimeseriesChartDatapointDescriptor> Datapoints
);
