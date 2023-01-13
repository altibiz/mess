using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Mess.Timeseries;

public class AdminMenu : INavigationProvider
{
  public Task BuildNavigationAsync(string name, NavigationBuilder builder)
  {
    if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
    {
      return Task.CompletedTask;
    }

    builder.Add(
      S["Data"],
      data =>
        data.Add(
          S["Timeseries"],
          S["Timeseries"].PrefixPosition(),
          entry =>
            entry
              .AddClass("timeseries")
              .Id("timeseries")
              .Action("Index", "Admin", new { area = "Mess.Timeseries" })
              .LocalNav()
        )
    );

    return Task.CompletedTask;
  }

  public AdminMenu(IStringLocalizer<AdminMenu> localizer)
  {
    S = localizer;
  }

  private IStringLocalizer S { get; }
}
