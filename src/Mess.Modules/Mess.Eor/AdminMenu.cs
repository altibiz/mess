using Mess.Eor.Controllers;
using Microsoft.Extensions.Localization;
using OrchardCore.Contents;
using OrchardCore.Contents.Security;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;

namespace Mess.Eor;

public class AdminMenu : INavigationProvider
{
  public Task BuildNavigationAsync(string name, NavigationBuilder builder)
  {
    if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
    {
      return Task.CompletedTask;
    }

    builder.Add(
      S["EOR"],
      S["EOR"].PrefixPosition(),
      eor =>
        eor.Add(
          S["Devices"],
          S["Devices"].PrefixPosition(),
          devices =>
            devices
              .AddClass("devices")
              .Id("devices")
              .Action(
                nameof(EorMeasurementDeviceAdminController.List),
                typeof(EorMeasurementDeviceAdminController).ControllerName(),
                "Mess.Eor"
              )
              .Permission(
                ContentTypePermissionsHelper.CreateDynamicPermission(
                  CommonPermissions.ViewOwnContent,
                  "EorMeasurementDevice"
                )
              )
              .LocalNav()
        )
    );

    return Task.CompletedTask;
  }

  public AdminMenu(IStringLocalizer<AdminMenu> localizer)
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
