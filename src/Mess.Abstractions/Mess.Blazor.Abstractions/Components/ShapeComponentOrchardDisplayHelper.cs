using OrchardCore;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Razor;

namespace Mess.Blazor.Abstractions.Components;

internal class ShapeComponentOrchardDisplayHelper : IOrchardDisplayHelper,
  IOrchardHelper
{
  public ShapeComponentOrchardDisplayHelper(HttpContext context,
    IDisplayHelper displayHelper)
  {
    HttpContext = context;
    DisplayHelper = displayHelper;
  }

  public HttpContext HttpContext { get; set; }

  public IDisplayHelper DisplayHelper { get; set; }
}
