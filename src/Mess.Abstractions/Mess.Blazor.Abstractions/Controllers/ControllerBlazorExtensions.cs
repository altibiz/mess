using System.Text.Encodings.Web;
using Mess.Blazor.Abstractions.Components;
using Mess.Blazor.Abstractions.ViewModels;
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
    return controller.PartialView(
      "ComponentView.cshtml",
      new ComponentViewModel
      {
        ComponentType = componentType,
        Model = model
      }
    );
  }
}
