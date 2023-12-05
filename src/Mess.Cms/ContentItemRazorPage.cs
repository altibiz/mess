using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Razor;

namespace Mess.Cms;

public abstract class ContentItemRazorPage<TContentItem> : RazorPage<dynamic>
  where TContentItem : ContentItemBase
{
  public ContentItem WeakContentItem => Model.ContentItem;

  public TContentItem ContentItem => WeakContentItem.AsContent<TContentItem>();
}
