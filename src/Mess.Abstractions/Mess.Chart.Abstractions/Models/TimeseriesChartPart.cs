using Etch.OrchardCore.Fields.MultiSelect.Fields;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartPart : ContentPart
{
  public string ChartDataProviderId { get; set; } = default!;

  public MultiSelectField History { get; set; } = default!;

  public MultiSelectField RefreshInterval { get; set; } = default!;

  public List<ContentItem> Datasets { get; set; } = default!;

  [JsonIgnore]
  public TimeSpan HistorySpan =>
    History?.SelectedValues.FirstOrDefault() switch
    {
      "5 minutes" => TimeSpan.FromMinutes(5),
      "10 minutes" => TimeSpan.FromMinutes(10),
      "15 minutes" => TimeSpan.FromMinutes(15),
      "30 minutes" => TimeSpan.FromMinutes(30),
      "1 hour" => TimeSpan.FromHours(1),
      "1 day" => TimeSpan.FromDays(1),
      "1 week" => TimeSpan.FromDays(7),
      "1 month" => TimeSpan.FromDays(30),
      "1 year" => TimeSpan.FromDays(365),
      _ => TimeSpan.FromMinutes(15),
    };

  [JsonIgnore]
  public TimeSpan RefreshIntervalSpan =>
    RefreshInterval?.SelectedValues.FirstOrDefault() switch
    {
      "10 seconds" => TimeSpan.FromSeconds(10),
      "30 seconds" => TimeSpan.FromSeconds(30),
      "1 minute" => TimeSpan.FromMinutes(1),
      "5 minutes" => TimeSpan.FromMinutes(5),
      "10 minutes" => TimeSpan.FromMinutes(10),
      "15 minutes" => TimeSpan.FromMinutes(15),
      "30 minutes" => TimeSpan.FromMinutes(30),
      "1 hour" => TimeSpan.FromHours(1),
      _ => TimeSpan.FromMinutes(1),
    };
}
