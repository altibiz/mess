using Mess.Fields.Abstractions.Fields;
using Mess.Fields.Abstractions.ApiKeys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;

namespace Mess.Iot.Abstractions.Services;

public abstract class ApiKeyIotAuthorizationHandler<T>
  : IIotAuthorizationHandler
  where T : ContentItemBase
{
  public abstract ApiKeyField GetApiKey(T contentItem);

  public void Authorize(
    AuthorizationFilterContext context,
    ContentItem contentItem
  )
  {
    var item = contentItem.AsContent<T>();

    var apiKey = context.HttpContext.Request.Headers[
      "X-API-Key"
    ].FirstOrDefault();
    if (apiKey is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    var apiKeyField = GetApiKey(item);

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
    ContentItem contentItem
  )
  {
    var item = contentItem.AsContent<T>();
    if (item is null)
    {
      context.Result = new NotFoundResult();
      return;
    }

    var apiKey = context.HttpContext.Request.Headers[
      "X-API-Key"
    ].FirstOrDefault();
    if (apiKey is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    var apiKeyField = GetApiKey(item);
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

  public string ContentType => typeof(T).ContentTypeName();
}
