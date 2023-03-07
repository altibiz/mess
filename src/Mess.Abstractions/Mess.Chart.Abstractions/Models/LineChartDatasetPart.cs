using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using Etch.OrchardCore.Fields.Colour.Fields;

namespace Mess.Chart.Abstractions.Models;

public class LineChartDatasetPart : NestedChartPart
{
  public TextField Label { get; set; } = default!;

  public ColourField Color { get; set; } = default!;

  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
  public ContentItem? Dataset { get; set; } = default!;
}
