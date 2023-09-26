using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Security;
using Mess.Iot.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.Iot.Filters;

[AttributeUsage(
  AttributeTargets.Class | AttributeTargets.Method,
  Inherited = true
)]
public class MeasurementDeviceAuthorization
  : Attribute,
    IAsyncAuthorizationFilter
{
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    if (context.RouteData.Values["deviceId"] is not string deviceId)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var cache =
      context.HttpContext.RequestServices.GetRequiredService<IMeasurementDeviceContentItemCache>();
    var contentItem = await cache.GetAsync(deviceId);
    if (contentItem is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var handler =
      context.HttpContext.RequestServices
        .GetServices<IMeasurementDeviceAuthorizationHandler>()
        .FirstOrDefault(
          handler => handler.ContentType == contentItem.ContentType
        )
      ?? throw new NotImplementedException(
        "No measurement device authorization handler for "
          + contentItem.ContentType
      );

    await handler.AuthorizeAsync(context, contentItem);
  }
}
