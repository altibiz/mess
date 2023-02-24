using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class ChartPart : ContentPart
{
  public string DataProviderId { get; set; } = default!;

  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
  public ContentItem? Chart { get; set; } = default!;
}
