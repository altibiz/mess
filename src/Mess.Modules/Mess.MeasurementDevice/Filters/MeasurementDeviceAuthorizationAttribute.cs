using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Models;
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
    var deviceId = context.RouteData.Values["deviceId"] as string;
    if (deviceId is null)
    {
      context.Result = new UnauthorizedResult();
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
      context.Result = new UnauthorizedResult();
      return;
    }

    var handler = context.HttpContext.RequestServices
      .GetServices<IMeasurementDeviceAuthorizationHandler>()
      .FirstOrDefault(
        handler => handler.ContentType == contentItem.ContentType
      );
    if (handler is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    await handler.AuthorizeAsync(context, contentItem);
  }
}
