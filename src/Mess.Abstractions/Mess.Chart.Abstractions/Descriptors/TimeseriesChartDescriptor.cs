namespace Mess.Chart.Abstractions.Descriptors;

public record TimeseriesChartDescriptor(
  TimeSpan RefreshInterval,
  TimeSpan History,
  IReadOnlyCollection<TimeseriesChartDatasetDescriptor> Datasets
) : ChartDescriptor("timeseries", RefreshInterval);
