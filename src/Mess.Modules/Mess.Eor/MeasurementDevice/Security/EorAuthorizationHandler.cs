using Mess.ContentFields.Abstractions.Services;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;

namespace Mess.Eor.MeasurementDevice.Security;

public class EorAuthorizationHandler : IMeasurementDeviceAuthorizationHandler
{
  public const string AuthorizationContentType = "EorMeasurementDevice";

  public string ContentType => AuthorizationContentType;

  public void Authorize(
    AuthorizationFilterContext context,
    ContentItem measurementDevice
  )
  {
    var apiKey = context.HttpContext.Request.Headers[
      "X-API-Key"
    ].FirstOrDefault();
    if (apiKey is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    var eorMeasurementDevice = measurementDevice.As<EorMeasurementDevicePart>();
    if (eorMeasurementDevice is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var apiKeyFieldService =
      context.HttpContext.RequestServices.GetRequiredService<IApiKeyFieldService>();
    var authorized = apiKeyFieldService.Authorize(
      eorMeasurementDevice.ApiKey,
      apiKey
    );
    if (!authorized)
    {
      context.Result = new UnauthorizedResult();
      return;
    }
  }

  public async Task AuthorizeAsync(
    AuthorizationFilterContext context,
    ContentItem measurementDevice
  )
  {
    var apiKey = context.HttpContext.Request.Headers[
      "X-API-Key"
    ].FirstOrDefault();
    if (apiKey is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    var eorMeasurementDevice = measurementDevice.As<EorMeasurementDevicePart>();
    if (eorMeasurementDevice is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var apiKeyFieldService =
      context.HttpContext.RequestServices.GetRequiredService<IApiKeyFieldService>();
    var authorized = await apiKeyFieldService.AuthorizeAsync(
      eorMeasurementDevice.ApiKey,
      apiKey
    );
    if (!authorized)
    {
      context.Result = new UnauthorizedResult();
      return;
    }
  }
}
