using Mess.Tenants;

namespace Mess.OrchardCore.Extensions.Microsoft;

public static class IConfigurationSectionExtension
{
  public static Tenant GetTenant(this IConfigurationSection section) =>
    new(
      Name: GetTenantNameFromIConfigurationSection(section),
      ConnectionString: GetTenantConnectionStringFromIConfigurationSection(
        section
      ),
      TablePrefix: GetTenantTablePrefixFromIConfigurationSection(section)
    );

  private static string GetTenantNameFromIConfigurationSection(
    IConfigurationSection settings
  ) =>
    settings.GetValue<string>("ShellName")
    ?? throw new InvalidOperationException("Tenant must have a name");

  private static string GetTenantConnectionStringFromIConfigurationSection(
    IConfigurationSection settings
  ) =>
    settings.GetValue<string>("DatabaseConnectionString")
    ?? throw new InvalidOperationException(
      $"Tenant must have a connection string"
    );

  private static string GetTenantTablePrefixFromIConfigurationSection(
    IConfigurationSection settings
  ) =>
    settings.GetValue<string>("DatabaseTablePrefix")
    ?? throw new InvalidOperationException($"Tenant must have a table prefix");
}
