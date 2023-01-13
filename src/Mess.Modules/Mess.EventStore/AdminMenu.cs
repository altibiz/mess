using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Mess.EventStore;

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
          S["Event Store"],
          S["Event Store"].PrefixPosition(),
          entry =>
            entry
              .AddClass("event-store")
              .Id("event-store")
              .Action("Index", "Admin", new { area = "Mess.EventStore" })
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
