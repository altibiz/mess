using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Environment.Shell;

namespace Mess.Util.Extensions.OrchardCore;

public static class ShellScopeExtensions
{
  public static string GetTenantName(this ShellScope shellScope) =>
    shellScope?.ShellContext?.Settings?.Name
    ?? throw new InvalidOperationException("Tenant must be activated");

  public static string GetTenantConnectionString(this ShellScope shellScope) =>
    shellScope?.ShellContext?.Settings?.ShellConfiguration?["ConnectionString"]
    ?? throw new InvalidOperationException($"Tenant must be activated");

  public static string GetTenantName(this ShellSettings settings) =>
    settings?.Name
    ?? throw new InvalidOperationException("Tenant must be activated");

  public static string GetTenantConnectionString(this ShellSettings settings) =>
    settings?.ShellConfiguration?["ConnectionString"]
    ?? throw new InvalidOperationException($"Tenant must be activated");
}
