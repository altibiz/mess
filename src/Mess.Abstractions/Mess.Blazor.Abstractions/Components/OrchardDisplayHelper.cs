using OrchardCore;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Razor;

namespace Mess.Blazor.Abstractions.Components;

internal class OrchardDisplayHelper : IOrchardDisplayHelper, IOrchardHelper
{
  public HttpContext HttpContext { get; set; }

  public IDisplayHelper DisplayHelper { get; set; }

  public OrchardDisplayHelper(HttpContext context, IDisplayHelper displayHelper)
  {
    HttpContext = context;
    DisplayHelper = displayHelper;
  }
}
