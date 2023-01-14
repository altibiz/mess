using OrchardCore.ContentManagement;

namespace Mess.Chart.Fields;

public class ChartField : ContentField
{
  public string Type { get; set; } = default!;
}
