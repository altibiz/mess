using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.MeasurementDevice.Filters;

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

    var session =
      context.HttpContext.RequestServices.GetRequiredService<ISession>();
    var contentItem = await session
      .Query<ContentItem, MeasurementDeviceIndex>()
      .Where(index => index.DeviceId == deviceId)
      .FirstOrDefaultAsync();
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
