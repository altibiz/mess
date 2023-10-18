using Mess.Ozds.Controllers;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;

namespace OrchardCore.Cors
{
  public class AdminMenu : INavigationProvider
  {
    public AdminMenu(IStringLocalizer<AdminMenu> localizer)
    {
      S = localizer;
    }

    public IStringLocalizer S { get; set; }

    public Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
      if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
      {
        return Task.CompletedTask;
      }

      builder.Add(
        S["OZDS"],
        configuration =>
          configuration
            .Add(
              S["IOT devices"],
              S["IOT devices"].PrefixPosition(),
              entry =>
                entry
                  .AddClass("iot-devices")
                  .Id("iot-devices")
                  .Action(
                    nameof(OzdsIotDeviceAdminController.List),
                    typeof(OzdsIotDeviceAdminController).ControllerName(),
                    new { area = "Mess.Ozds" }
                  )
                  .LocalNav()
            )
            .Add(
              S["Distribution system units"],
              S["Distribution system units"].PrefixPosition(),
              entry =>
                entry
                  .AddClass("distribution-system-units")
                  .Id("distribution-system-units")
                  .Action(
                    nameof(DistributionSystemUnitController.List),
                    typeof(DistributionSystemUnitController).ControllerName(),
                    new { area = "Mess.Ozds" }
                  )
                  .LocalNav()
            )
            .Add(
              S["Closed distribution systems"],
              S["Closed distribution systems"].PrefixPosition(),
              entry =>
                entry
                  .AddClass("closed-distribution-systems")
                  .Id("closed-distribution-systems")
                  .Action(
                    nameof(ClosedDistributionSystemAdminController.List),
                    typeof(ClosedDistributionSystemAdminController).ControllerName(),
                    new { area = "Mess.Ozds" }
                  )
                  .LocalNav()
            )
            .Add(
              S["Distribution system operators"],
              S["Distribution system operators"].PrefixPosition(),
              entry =>
                entry
                  .AddClass("distribution-system-operators")
                  .Id("distribution-system-operators")
                  .Action(
                    nameof(DistributionSystemOperatorController.List),
                    typeof(DistributionSystemOperatorController).ControllerName(),
                    new { area = "Mess.Ozds" }
                  )
                  .LocalNav()
            )
      );

      return Task.CompletedTask;
    }
  }
}
