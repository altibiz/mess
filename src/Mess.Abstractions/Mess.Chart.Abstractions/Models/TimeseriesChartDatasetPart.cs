using Etch.OrchardCore.Fields.MultiSelect.Fields;
using Newtonsoft.Json;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetPart : NestedChartPart
{
  public string Property { get; set; } = default!;

  public MultiSelectField History { get; set; } = default!;

  [JsonIgnore]
  public TimeSpan HistoryTimeSpan =>
    History.SelectedValues.FirstOrDefault() switch
    {
      "Hour" => TimeSpan.FromHours(1),
      "Day" => TimeSpan.FromDays(1),
      "Week" => TimeSpan.FromDays(7),
      "Month" => TimeSpan.FromDays(30),
      "Year" => TimeSpan.FromDays(365),
      _ => TimeSpan.FromHours(1)
    };
}
