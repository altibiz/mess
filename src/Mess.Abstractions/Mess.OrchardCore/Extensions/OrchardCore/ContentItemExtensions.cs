using OrchardCore.ContentManagement;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class ContentItemExtensions
{
  public static ContentItem Merge(
    this ContentItem contentItem,
    ContentItem other
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
