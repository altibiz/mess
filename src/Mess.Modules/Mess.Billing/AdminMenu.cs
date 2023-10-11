using Mess.Billing.Abstractions;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Mess.Billing;

public class AdminMenu : INavigationProvider
{
  public Task BuildNavigationAsync(string name, NavigationBuilder builder)
  {
    if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
    {
      return Task.CompletedTask;
    }

    builder.Add(
      S["Billing"],
      configuration =>
        configuration
          .Add(
            S["Payments"],
            S["Payments"].PrefixPosition(),
            entry =>
              entry
                .AddClass("payments")
                .Id("payments")
                .Action("Payments", "Admin", new { area = "Mess.Billing" })
                .Permission(Permissions.ListPayments)
                .LocalNav()
          )
          .Add(
            S["Payments"],
            S["Payments"].PrefixPosition(),
            entry =>
              entry
                .AddClass("payments")
                .Id("payments")
                .Action("OwnPayments", "Admin", new { area = "Mess.Billing" })
                .Permission(Permissions.ListOwnPayments)
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
