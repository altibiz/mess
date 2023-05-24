using Etch.OrchardCore.Fields.Colour.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetPart : ContentPart
{
  public TextField Label { get; set; } = default!;

  public ColourField Color { get; set; } = default!;

  public string Property { get; set; } = default!;
}
