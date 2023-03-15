using Etch.OrchardCore.Fields.MultiSelect.Fields;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartPart : ContentPart
{
  public string DataProviderId { get; set; } = default!;

  public MultiSelectField History { get; set; } = default!;

  public MultiSelectField Interval { get; set; } = default!;

  public List<ContentItem> Datasets { get; set; } = default!;

  [JsonIgnore]
  public TimeSpan HistorySpan => TimeSpan.Parse(History.SelectedValues.First());

  [JsonIgnore]
  public TimeSpan IntervalSpan =>
    TimeSpan.Parse(Interval.SelectedValues.First());
}
