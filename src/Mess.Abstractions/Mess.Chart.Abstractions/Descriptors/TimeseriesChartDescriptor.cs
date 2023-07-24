namespace Mess.Chart.Abstractions.Descriptors;

public record TimeseriesChartDescriptor(
  double RefreshInterval,
  double History,
  IReadOnlyCollection<TimeseriesChartDatasetDescriptor> Datasets
) : ChartDescriptor("timeseries", RefreshInterval);
