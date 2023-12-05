using Etch.OrchardCore.Fields.Colour;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Mess.Fields;

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

    builder.Remove(item => item.Id == "colours");
    builder.Add(
      S["Configuration"],
      configuration =>
        configuration.Add(
          S["Settings"],
          settings =>
            settings.Add(
              S["Colours"],
              S["Colours"].PrefixPosition(),
              layers =>
                layers
                  .AddClass("colours")
                  .Id("colours")
                  .Action(
                    "Index",
                    "Admin",
                    new
                    {
                      area = "OrchardCore.Settings",
                      groupId = Constants.GroupId
                    }
                  )
                  .Permission(Permissions.ManageColourSettings)
                  .LocalNav()
            )
        )
    );

    return Task.CompletedTask;
  }
}
