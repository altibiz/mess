using Mess.Tenants;
using OrchardCore.Environment.Shell;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class ShellSettingsExtensions
{
  public static Tenant GetTenant(this ShellSettings settings) =>
    new(
      Name: GetTenantNameFromShellSettings(settings),
      ConnectionString: GetTenantConnectionStringFromShellSettings(settings),
      TablePrefix: GetTenantTablePrefixFromShellSettings(settings)
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

  private static string GetTenantTablePrefixFromShellSettings(
    ShellSettings settings
  ) =>
    settings?.ShellConfiguration?["TablePrefix"]
    ?? throw new InvalidOperationException($"Tenant must have a table prefix");
}
