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
    return await Component(controller, typeof(TComponent), model);
  }

  public static async Task<IActionResult> Component(
    this Controller controller,
    Type componentType,
    object? model
  )
  {
    var httpContext = controller.HttpContext;

    var shapeTemplateComponentEngine = httpContext.RequestServices.GetRequiredService<IShapeTemplateComponentEngine>();

    using var writer = new StringWriter();
    var htmlContent = await shapeTemplateComponentEngine.RenderAsync(componentType, "", model, writer);
    htmlContent.WriteTo(writer, HtmlEncoder.Default);
    var htmlString = writer.ToString();

    return controller.Content(htmlString, "text/html");
  }
}
