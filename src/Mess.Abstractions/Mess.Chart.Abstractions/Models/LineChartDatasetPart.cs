using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class LineChartDatasetPart : NestedChartPart
{
  public string Label { get; set; } = default!;

  public string Color { get; set; } = default!;

  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
  public ContentItem? Dataset { get; set; } = default!;
}
