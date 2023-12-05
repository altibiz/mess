using Mess.Eor.Controllers;
using Microsoft.Extensions.Localization;
using OrchardCore.Contents;
using OrchardCore.Contents.Security;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;

namespace Mess.Eor;

public class AdminMenu : INavigationProvider
{
  private readonly IStringLocalizer S;

  public AdminMenu(IStringLocalizer<AdminMenu> localizer)
  {
    S = localizer;
  }

  public Task BuildNavigationAsync(string name, NavigationBuilder builder)
  {
    if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
      return Task.CompletedTask;

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
                nameof(EorIotDeviceAdminController.List),
                typeof(EorIotDeviceAdminController).ControllerName(),
                "Mess.Eor"
              )
              .Permission(
                ContentTypePermissionsHelper.CreateDynamicPermission(
                  CommonPermissions.ViewOwnContent,
                  "EorIotDevice"
                )
              )
              .LocalNav()
        )
    );

    return Task.CompletedTask;
  }
}
