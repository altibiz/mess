using Mess.Cms.Extensions.OrchardCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using OrchardCore.Environment.Shell;
using OrchardCore.ResourceManagement;

// TODO: extract mudblazor into its own feature
// TODO: fix double rendering in non-component layouts

namespace Mess.Blazor;

public class ResourceMiddleware
{
  private readonly RequestDelegate _next;

  public ResourceMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(
    HttpContext context,
    IResourceManager resourceManager,
    ShellSettings shellSettings
  )
  {
    resourceManager.RegisterLink(new LinkEntry()
      .AddAttribute("href",
        "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap")
      .AddAttribute("rel", "stylesheet")
    );
    resourceManager.RegisterLink(new LinkEntry()
      .AddAttribute("href", "Mess.Blazor/MudBlazor.min.css")
      .AddAttribute("rel", "stylesheet")
    );
    resourceManager.RegisterFootScript(
      new HtmlString(
        "<script src=\"Mess.Blazor/MudBlazor.min.js\"></script>"
      )
    );

    resourceManager.RegisterHeadScript(
      new HtmlString(
        $"<base href=\"/{shellSettings.GetRequestUrlPrefix()}/\" />"
      )
    );
    resourceManager.RegisterFootScript(
      new HtmlString(
        "<script src=\"_framework/blazor.server.js\"></script>"
      )
    );

    await _next(context);
  }
}
