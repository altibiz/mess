using Microsoft.AspNetCore.Components;
using OrchardCore.ResourceManagement;

namespace Mess.Blazor.Abstractions.Extensions;

public static class ResourceManagerExtensions
{
  public static MarkupString RenderHeadLink(this IResourceManager resourceManager)
  {
    using var writer = new StringWriter();
    resourceManager.RenderHeadLink(writer);
    return new MarkupString(writer.ToString());
  }

  public static MarkupString RenderMeta(this IResourceManager resourceManager)
  {
    using var writer = new StringWriter();
    resourceManager.RenderMeta(writer);
    return new MarkupString(writer.ToString());
  }

  public static MarkupString RenderStylesheet(this IResourceManager resourceManager)
  {
    using var writer = new StringWriter();
    resourceManager.RenderStylesheet(writer);
    return new MarkupString(writer.ToString());
  }


  public static MarkupString RenderHeadScript(this IResourceManager resourceManager)
  {
    using var writer = new StringWriter();
    resourceManager.RenderHeadScript(writer);
    return new MarkupString(writer.ToString());
  }

  public static MarkupString RenderFootScript(this IResourceManager resourceManager)
  {
    using var writer = new StringWriter();
    resourceManager.RenderFootScript(writer);
    return new MarkupString(writer.ToString());
  }
}
