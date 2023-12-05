using Mess.Cms;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public abstract class IotAuthorizationHandler<T> : IIotAuthorizationHandler
  where T : ContentItemBase
{
  protected abstract void Authorize(
    AuthorizationFilterContext context,
    T contentItem
  );

  protected abstract Task AuthorizeAsync(
    AuthorizationFilterContext context,
    T contentItem
  );

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == typeof(T).ContentTypeName();
  }

  public void Authorize(
    AuthorizationFilterContext context,
    ContentItem contentItem
  )
  {
    var item = contentItem.AsContent<T>();
    Authorize(context, item);
  }

  public async Task AuthorizeAsync(
    AuthorizationFilterContext context,
    ContentItem contentItem
  )
  {
    var item = contentItem.AsContent<T>();
    await AuthorizeAsync(context, item);
  }
}
