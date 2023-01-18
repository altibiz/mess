using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;

namespace Mess.Chart.Abstractions.Providers;

public interface IChartProvider
{
  public string Id { get; }

  public ChartSpecification? CreateChart(ChartParameters parameters);

  public Task<ChartSpecification?> CreateChartAsync(ChartParameters parameters);

  public string? ValidateParameters(ChartParameters parameters);

  public Task<string?> ValidateParametersAsync(ChartParameters parameters);

  public string GetPartEditorShapeType(BuildPartEditorContext context);

  public object CreatePartEditorModel(
    BuildPartEditorContext context,
    ContentPart part,
    ChartParameters parameters
  );
}
