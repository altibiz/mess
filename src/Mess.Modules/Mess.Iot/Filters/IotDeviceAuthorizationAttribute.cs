using Mess.Iot.Abstractions.Services;
using Mess.Iot.Abstractions.Caches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.Iot.Filters;

[AttributeUsage(
  AttributeTargets.Class | AttributeTargets.Method,
  Inherited = true
)]
public class IotDeviceAuthorization : Attribute, IAsyncAuthorizationFilter
{
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    if (context.RouteData.Values["deviceId"] is not string deviceId)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var cache =
      context.HttpContext.RequestServices.GetRequiredService<IIotDeviceContentItemCache>();
    var contentItem = await cache.GetIotDeviceAsync(deviceId);
    if (contentItem is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var handler =
      context.HttpContext.RequestServices
        .GetServices<IIotAuthorizationHandler>()
        .FirstOrDefault(handler => handler.IsApplicable(contentItem))
      ?? throw new NotImplementedException(
        "No measurement device authorization handler for "
          + contentItem.ContentType
      );

    await handler.AuthorizeAsync(context, contentItem);
  }
}
