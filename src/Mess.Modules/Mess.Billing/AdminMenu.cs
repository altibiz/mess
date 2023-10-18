using Mess.Billing.Abstractions;
using Mess.Billing.Controllers;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
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
            S["Issued"],
            S["Issued"].PrefixPosition(),
            entry =>
              entry
                .AddClass("issued-bills")
                .Id("issued-bills")
                .Action(
                  nameof(AdminController.Bills),
                  typeof(AdminController).ControllerName(),
                  new { area = "Mess.Billing" }
                )
                .Permission(Permissions.ListIssuedBills)
                .LocalNav()
          )
          .Add(
            S["Received"],
            S["Received"].PrefixPosition(),
            entry =>
              entry
                .AddClass("received-bills")
                .Id("received-bills")
                .Action(
                  nameof(AdminController.Bills),
                  typeof(AdminController).ControllerName(),
                  new { area = "Mess.Billing" }
                )
                .Permission(Permissions.ListReceivedBills)
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
