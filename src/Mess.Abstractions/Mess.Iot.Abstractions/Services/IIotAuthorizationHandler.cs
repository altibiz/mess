using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public interface IIotAuthorizationHandler
{
  public string ContentType { get; }

  public Task AuthorizeAsync(
    AuthorizationFilterContext context,
    ContentItem measurementDevice
  );

  public void Authorize(
    AuthorizationFilterContext context,
    ContentItem measurementDevice
  );
}
