using Mess.Cms.Extensions.OrchardCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using OrchardCore.Environment.Shell;
using OrchardCore.ResourceManagement;

namespace Mess.Blazor;

public class ScriptMiddleware
{
  private readonly RequestDelegate _next;

  public ScriptMiddleware(RequestDelegate next)
  {
      _next = next;
  }

  public async Task InvokeAsync(
    HttpContext context,
    IResourceManager resourceManager,
    ShellSettings shellSettings
  )
  {
    resourceManager.RegisterFootScript(
      new HtmlString(
        "<script src='/"
          + shellSettings.GetRequestUrlPrefix()
          + "/_framework/blazor.server.js'></script>"
      )
    );

    await _next(context);
  }
}
