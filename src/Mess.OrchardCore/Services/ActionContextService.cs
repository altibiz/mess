using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OrchardCore.Admin;

namespace Mess.OrchardCore.Services;

public class ActionContextService : IActionContextService
{
  public bool IsAdmin
  {
    get
    {
      var actionDescriptor =
        _actionContextAccessor.ActionContext?.ActionDescriptor
        as ControllerActionDescriptor;

      if (actionDescriptor is null)
      {
        throw new InvalidOperationException("ActionDescriptor missing");
      }

      return actionDescriptor.ControllerTypeInfo
        .GetCustomAttributes(inherit: true)
        .Any(a => a.GetType() == typeof(AdminAttribute));
    }
  }

  public ActionContextService(IActionContextAccessor actionContextAccessor)
  {
    _actionContextAccessor = actionContextAccessor;
  }

  private readonly IActionContextAccessor _actionContextAccessor;
}
