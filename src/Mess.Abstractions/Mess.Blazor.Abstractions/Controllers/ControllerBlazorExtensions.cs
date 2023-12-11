using System.Text.Encodings.Web;
using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Abstractions.Controllers;

public static class ControllerBlazorExtensions
{
  public static async Task<IActionResult> Component<TComponent, TModel>(
    this Controller controller,
    TModel model
  ) where TComponent : ShapeComponentBase<TModel>
    where TModel : class
  {
    var httpContext = controller.HttpContext;

    var shapeComponentModelStore =
      httpContext.RequestServices.GetRequiredService<IShapeComponentModelStore>();
    var renderId = Guid.NewGuid();
    shapeComponentModelStore.Add(renderId, model);

    var htmlHelper =
      httpContext.RequestServices.GetRequiredService<IHtmlHelper>();
    var htmlContent = await htmlHelper.RenderComponentAsync<TComponent>(
      RenderMode.ServerPrerendered,
      new { RenderId = renderId }
    );

    using var writer = new StringWriter();
    htmlContent.WriteTo(writer, HtmlEncoder.Default);
    var htmlString = writer.ToString();

    return controller.Content(htmlString, "text/html");
  }

  public static async Task<IActionResult> Component(
    this Controller controller,
    Type componentType,
    object? model
  )
  {
    var httpContext = controller.HttpContext;

    var shapeComponentModelStore =
      httpContext.RequestServices.GetRequiredService<IShapeComponentModelStore>();
    var renderId = Guid.NewGuid();
    shapeComponentModelStore.Add(renderId, model);

    var htmlHelper =
      httpContext.RequestServices.GetRequiredService<IHtmlHelper>();
    var htmlContent = await htmlHelper.RenderComponentAsync(
      componentType,
      RenderMode.ServerPrerendered,
      new { RenderId = renderId }
    );

    using var writer = new StringWriter();
    htmlContent.WriteTo(writer, HtmlEncoder.Default);
    var htmlString = writer.ToString();

    return controller.Content(htmlString, "text/html");
  }
}
