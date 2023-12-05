using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.Cms.Extensions.OrchardCore;

public static class ContentItemExtensions
{
  public static ContentItemBase Merge(
    this ContentItemBase contentItem,
    ContentItemBase other
  )
  {
    global::OrchardCore.ContentManagement.ContentItemExtensions.Merge(
      contentItem.Inner.Merge(other.Inner),
      other.Inner
    );
    contentItem.Inner.DisplayText = other.Inner.DisplayText;
    return contentItem;
  }

  public static OrchardContentItem Merge(
    this OrchardContentItem contentItem,
    OrchardContentItem other
  )
  {
    global::OrchardCore.ContentManagement.ContentItemExtensions.Merge(
      contentItem,
      other
    );
    contentItem.DisplayText = other.DisplayText;
    return contentItem;
  }
}
