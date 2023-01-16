using Mess.Tenants;
using OrchardCore.Environment.Shell;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class ShellSettingsExtensions
{
  public static Tenant GetTenant(this ShellSettings settings) =>
    new(
      GetTenantNameFromShellSettings(settings),
      GetTenantConnectionStringFromShellSettings(settings)
    );

  private static string GetTenantNameFromShellSettings(
    ShellSettings settings
  ) =>
    settings.Name
    ?? throw new InvalidOperationException("Tenant must have a name");

  private static string GetTenantConnectionStringFromShellSettings(
    ShellSettings settings
  ) =>
    settings?.ShellConfiguration?["ConnectionString"]
    ?? throw new InvalidOperationException(
      $"Tenant must have a connection string"
    );
}
