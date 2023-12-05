using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Title.Models;

namespace Mess.Chart.Abstractions.Models;

public class DashboardItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<FlowPart> FlowPart { get; private set; } = default!;

  private DashboardItem(ContentItem contentItem)
    : base(contentItem) { }
}
