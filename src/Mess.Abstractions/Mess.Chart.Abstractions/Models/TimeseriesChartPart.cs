using Etch.OrchardCore.Fields.MultiSelect.Fields;
using Mess.ContentFields.Abstractions.Fields;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartPart : ContentPart
{
  public string ChartContentType { get; set; } = default!;

  public IntervalField History { get; set; } = default!;

  public IntervalField RefreshInterval { get; set; } = default!;

  public List<ContentItem> Datasets { get; set; } = default!;
}
