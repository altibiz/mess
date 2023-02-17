using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class LineChartPart : NestedChartPart
{
  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
  public List<ContentItem>? Datasets { get; set; } = default!;
}
