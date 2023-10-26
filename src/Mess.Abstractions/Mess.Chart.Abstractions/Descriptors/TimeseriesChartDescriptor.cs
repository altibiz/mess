namespace Mess.Chart.Abstractions.Descriptors;

public record TimeseriesChartDescriptor(
  decimal RefreshInterval,
  decimal History,
  IReadOnlyCollection<TimeseriesChartDatasetDescriptor> Datasets
) : ChartDescriptor("timeseries", RefreshInterval);
