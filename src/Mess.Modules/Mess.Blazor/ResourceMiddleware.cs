using Mess.Cms.Extensions.OrchardCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using OrchardCore.Environment.Shell;
using OrchardCore.ResourceManagement;

// TODO: extract mudblazor into its own feature

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
    // TODO: check that this doesn't break anything else
    resourceManager.RegisterHeadScript(
      new HtmlString(
        $"<base href='/{shellSettings.GetRequestUrlPrefix()}/' />"
      )
    );
    resourceManager.RegisterFootScript(
      new HtmlString(
        "<script src='/"
        + shellSettings.GetRequestUrlPrefix()
        + "/_framework/blazor.server.js'></script>"
      )
    );

    resourceManager.RegisterLink(new LinkEntry()
      .AddAttribute("href",
        "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap")
      .AddAttribute("rel", "stylesheet")
    );
    resourceManager.RegisterLink(new LinkEntry()
      .AddAttribute("href", "/_content/MudBlazor/MudBlazor.min.css")
      .AddAttribute("rel", "stylesheet")
    );
    resourceManager.RegisterFootScript(
      new HtmlString(
        "<script src='/_content/MudBlazor/MudBlazor.min.js'></script>"
      )
    );

    await _next(context);
  }
}
