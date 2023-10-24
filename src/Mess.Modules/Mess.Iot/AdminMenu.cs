using Mess.Iot.Abstractions;
using Mess.Iot.Controllers;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;

namespace Mess.Iot;

public class AdminMenu : INavigationProvider
{
  public Task BuildNavigationAsync(string name, NavigationBuilder builder)
  {
    if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
    {
      return Task.CompletedTask;
    }

    builder.Add(
      S["IOT"],
      configuration =>
        configuration
          .AddClass("icon-class-fas")
          .AddClass("icon-class-fa-circle-nodes")
          .Id("billing")
          .Add(
            S["Devices"],
            S["Devices"].PrefixPosition(),
            entry =>
              entry
                .AddClass("iot-devices")
                .Id("iot-devices")
                .Action(
                  nameof(AdminController.ListIotDevices),
                  typeof(AdminController).ControllerName(),
                  new { area = "Mess.Iot" }
                )
                .Permission(Permissions.ListIotDevices)
                .LocalNav()
          )
    );

    return Task.CompletedTask;
  }

  public AdminMenu(IStringLocalizer<AdminMenu> localizer)
  {
    S = localizer;
  }

  private IStringLocalizer S { get; set; }
}
