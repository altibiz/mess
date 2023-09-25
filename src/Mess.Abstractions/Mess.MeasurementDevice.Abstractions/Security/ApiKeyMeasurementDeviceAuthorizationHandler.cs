using Mess.ContentFields.Abstractions.Fields;
using Mess.ContentFields.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Security;

public abstract class ApiKeyMeasurementDeviceAuthorizationHandler
  : IMeasurementDeviceAuthorizationHandler
{
  public abstract string ContentType { get; }

  public abstract ApiKeyField? GetApiKey(ContentItem measurementDevice);

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

    var apiKeyField = GetApiKey(measurementDevice);
    if (apiKeyField is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var apiKeyFieldService =
      context.HttpContext.RequestServices.GetRequiredService<IApiKeyFieldService>();
    var authorized = apiKeyFieldService.Authorize(apiKeyField, apiKey);
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

    var apiKeyField = GetApiKey(measurementDevice);
    if (apiKeyField is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var apiKeyFieldService =
      context.HttpContext.RequestServices.GetRequiredService<IApiKeyFieldService>();
    var authorized = await apiKeyFieldService.AuthorizeAsync(
      apiKeyField,
      apiKey
    );
    if (!authorized)
    {
      context.Result = new UnauthorizedResult();
      return;
    }
  }
}
