using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Environment.Shell;

namespace Mess.Util.OrchardCore.Tenants;

public class ShellTenantProvider : ITenantProvider
{
  public string GetTenantName() => GetTenantName(ShellScope.Current);

  public string GetTenantConnectionString() =>
    GetTenantConnectionString(ShellScope.Current);

  public string GetTenantName(ShellScope shellScope) =>
    GetTenantName(shellScope?.ShellContext?.Settings);

  public string GetTenantConnectionString(ShellScope? shellScope) =>
    GetTenantConnectionString(shellScope?.ShellContext?.Settings);

  public string GetTenantName(ShellSettings? settings) =>
    settings?.Name
    ?? throw new InvalidOperationException("Tenant must be activated");

  public string GetTenantConnectionString(ShellSettings? settings) =>
    settings?.ShellConfiguration?["ConnectionString"]
    ?? throw new InvalidOperationException($"Tenant must be activated");
}
