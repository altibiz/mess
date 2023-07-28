using Etch.OrchardCore.Fields.MultiSelect.Fields;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartPart : ContentPart
{
  public string ChartContentType { get; set; } = default!;

  public MultiSelectField History { get; set; } = default!;

  public MultiSelectField RefreshInterval { get; set; } = default!;

  public List<ContentItem> Datasets { get; set; } = default!;

  [JsonIgnore]
  public TimeSpan HistorySpan
  {
    get =>
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
    set
    {
      if (History is null)
      {
        History = new MultiSelectField();
      }
      History.SelectedValues = new[]
      {
        value switch
        {
          var val when val == TimeSpan.FromSeconds(10) => "10 seconds",
          var val when val == TimeSpan.FromSeconds(30) => "30 seconds",
          var val when val == TimeSpan.FromMinutes(1) => "1 minute",
          var val when val == TimeSpan.FromMinutes(5) => "5 minutes",
          var val when val == TimeSpan.FromMinutes(10) => "10 minutes",
          var val when val == TimeSpan.FromMinutes(15) => "15 minutes",
          var val when val == TimeSpan.FromMinutes(30) => "30 minutes",
          var val when val == TimeSpan.FromHours(1) => "1 hour",
          _ => "1 minute"
        }
      };
    }
  }

  [JsonIgnore]
  public TimeSpan RefreshIntervalSpan
  {
    get =>
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
    set
    {
      if (RefreshInterval is null)
      {
        RefreshInterval = new MultiSelectField();
      }
      RefreshInterval.SelectedValues = new[]
      {
        value switch
        {
          var val when val == TimeSpan.FromSeconds(10) => "10 seconds",
          var val when val == TimeSpan.FromSeconds(30) => "30 seconds",
          var val when val == TimeSpan.FromMinutes(1) => "1 minute",
          var val when val == TimeSpan.FromMinutes(5) => "5 minutes",
          var val when val == TimeSpan.FromMinutes(10) => "10 minutes",
          var val when val == TimeSpan.FromMinutes(15) => "15 minutes",
          var val when val == TimeSpan.FromMinutes(30) => "30 minutes",
          var val when val == TimeSpan.FromHours(1) => "1 hour",
          _ => "1 minute"
        }
      };
    }
  }
}
