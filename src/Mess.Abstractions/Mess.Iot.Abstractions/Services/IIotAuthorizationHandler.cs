using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public interface IIotAuthorizationHandler
{
  public bool IsApplicable(ContentItem contentItem);

  public Task AuthorizeAsync(
    AuthorizationFilterContext context,
    ContentItem contentItem
  );

  public void Authorize(
    AuthorizationFilterContext context,
    ContentItem contentItem
  );
}
