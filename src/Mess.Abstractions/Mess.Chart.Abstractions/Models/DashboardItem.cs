using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Title.Models;

namespace Mess.Chart.Abstractions.Models;

public class DashboardItem : ContentItemBase
{
  private DashboardItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<FlowPart> FlowPart { get; private set; } = default!;
}
