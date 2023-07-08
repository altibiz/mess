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
public class MeasurementDeviceApiKeyAttribute
  : Attribute,
    IAsyncAuthorizationFilter
{
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    var apiKey = context.HttpContext.Request.Headers[
      "X-API-Key"
    ].FirstOrDefault();
    if (apiKey is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

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

    var measurementDevice = contentItem.As<MeasurementDevicePart>();
    if (measurementDevice is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    var measurementDeviceGuard =
      context.HttpContext.RequestServices.GetRequiredService<IMeasurementDeviceGuard>();
    var authorized = await measurementDeviceGuard.AuthorizeAsync(
      measurementDevice,
      apiKey
    );
    if (!authorized)
    {
      context.Result = new UnauthorizedResult();
      return;
    }
  }
}
