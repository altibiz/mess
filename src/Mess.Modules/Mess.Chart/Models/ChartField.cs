using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Models;

public class ChartField : ContentField
{
  public ChartParameters Parameters { get; set; } = default!;
}
