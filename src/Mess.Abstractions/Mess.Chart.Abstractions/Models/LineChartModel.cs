namespace Mess.Chart.Abstractions.Models;

public record LineChartModel(IReadOnlyList<LineChartDatasetModel> Datasets)
  : ChartModel(ChartType.Line);
