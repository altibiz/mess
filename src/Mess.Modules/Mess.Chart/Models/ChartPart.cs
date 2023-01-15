using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Chart.Models;

public class ChartPart : ContentPart
{
  public ChartParameters Parameters { get; set; } = default!;
}
