using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Fields;

public class ChartField : ContentField
{
  public ChartParameters Parameters { get; set; } = default!;
}
