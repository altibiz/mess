using Microsoft.Extensions.Localization;
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
              S["Measurement devices"],
              S["Measurement devices"].PrefixPosition(),
              entry =>
                entry
                  .AddClass("measurement-devices")
                  .Id("measurement-devices")
                  .Action(
                    "List",
                    "MeasurementDeviceAdmin",
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
                    "List",
                    "DistributionSystemUnitAdmin",
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
                    "List",
                    "ClosedDistributionSystemAdmin",
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
                    "List",
                    "DistributionSystemOperatorAdmin",
                    new { area = "Mess.Ozds" }
                  )
                  .LocalNav()
            )
      );

      return Task.CompletedTask;
    }
  }
}
