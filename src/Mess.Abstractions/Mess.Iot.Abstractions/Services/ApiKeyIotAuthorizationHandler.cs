using Mess.Cms;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions.Fields;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mess.Iot.Abstractions.Services;

public abstract class ApiKeyIotAuthorizationHandler<T>
  : IotAuthorizationHandler<T>
  where T : ContentItemBase
{
  protected abstract ApiKeyField GetApiKey(T contentItem);

  protected override void Authorize(
    AuthorizationFilterContext context,
    T contentItem
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

    var apiKeyField = GetApiKey(contentItem);

    var apiKeyFieldService =
      context.HttpContext.RequestServices
        .GetRequiredService<IApiKeyFieldService>();
    var authorized = apiKeyFieldService.Authorize(apiKeyField, apiKey);
    if (!authorized) context.Result = new UnauthorizedResult();
  }

  protected override async Task AuthorizeAsync(
    AuthorizationFilterContext context,
    T contentItem
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

    var apiKeyField = GetApiKey(contentItem);
    if (apiKeyField is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var apiKeyFieldService =
      context.HttpContext.RequestServices
        .GetRequiredService<IApiKeyFieldService>();
    var authorized = await apiKeyFieldService.AuthorizeAsync(
      apiKeyField,
      apiKey
    );
    if (!authorized) context.Result = new UnauthorizedResult();
  }
}
