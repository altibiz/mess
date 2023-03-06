using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class ContentItemExtensions
{
  public static ContentItem Merge(
    this ContentItem contentItem,
    ContentItem other
  )
  {
    global::OrchardCore.ContentManagement.ContentItemExtensions.Merge(
      contentItem.Inner,
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
