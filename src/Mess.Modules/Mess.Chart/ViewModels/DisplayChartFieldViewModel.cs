using Mess.Chart.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Chart.ViewModels;

public class DisplayChartFieldViewModel
{
  public string Type { get; set; } = default!;
  public ChartField Field { get; set; } = default!;
  public ContentPart Part { get; set; } = default!;
  public ContentPartFieldDefinition PartFieldDefinition { get; set; } =
    default!;
}
