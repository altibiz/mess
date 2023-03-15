using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;

namespace Mess.Chart.Abstractions.Models;

public class DashboardItem : ContentItemBase
{
  public const string ContentType = "Dashboard";

  public Lazy<FlowPart> Flow { get; set; } = default!;

  private DashboardItem(ContentItem contentItem) : base(contentItem) { }
}
