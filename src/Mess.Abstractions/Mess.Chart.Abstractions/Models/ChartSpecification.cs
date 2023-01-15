using System.Text.Json.Serialization;

namespace Mess.Chart.Abstractions.Models;

public record class ChartSpecification(
  ChartType Type,
  IChartTypeSpecification TypeSpecification
);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChartType
{
  Line,
  Doughnut
}

public interface IChartTypeSpecification
{
  public ChartType Type { get; }
}

public record class LineChartSpecification(
  IReadOnlyList<LineChartDataset> Datasets
) : IChartTypeSpecification
{
  public ChartType Type => ChartType.Line;
}

public record class LineChartDataset(
  string Label,
  string? Unit,
  string Color,
  IReadOnlyList<LineChartData> Data
);

public record class LineChartData(float X, float Y);
