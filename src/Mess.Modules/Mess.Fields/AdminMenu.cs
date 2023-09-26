using Microsoft.Extensions.Localization;
using Etch.OrchardCore.Fields.Colour;
using OrchardCore.Navigation;

namespace Mess.Fields;

public class AdminMenu : INavigationProvider
{
  public Task BuildNavigationAsync(string name, NavigationBuilder builder)
  {
    if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
    {
      return Task.CompletedTask;
    }

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

  public AdminMenu(IStringLocalizer<AdminMenu> localizer)
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
