using OrchardCore;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Razor;

namespace Mess.Blazor.Abstractions.Components;

internal class OrchardDisplayHelper : IOrchardDisplayHelper, IOrchardHelper
{
  public OrchardDisplayHelper(HttpContext context, IDisplayHelper displayHelper)
  {
    HttpContext = context;
    DisplayHelper = displayHelper;
  }

  public HttpContext HttpContext { get; set; }

  public IDisplayHelper DisplayHelper { get; set; }
}
