using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Environment.Shell;
using Mess.Tenants;
using Mess.OrchardCore.Extensions.OrchardCore;

namespace Mess.OrchardCore.Tenants;

public record class ShellTenantProvider(
  IShellSettingsManager ShellSettingsManager
) : ITenants
{
  public Tenant Current =>
    ShellScope.Current.ShellContext.Settings.GetTenant();

  public IReadOnlyList<Tenant> All =>
    ShellSettingsManager
      .LoadSettingsAsync()
      .Result.Select(ShellSettingsExtensions.GetTenant)
      .ToList();

  public void Impersonate(Tenant tenant, Action action)
  {
    _threadLocalTenant.Value = tenant;
    action();
    _threadLocalTenant.Value = null;
  }

  public async Task ImpersonateAsync(Tenant tenant, Func<Task> action)
  {
    await Task.Run(async () =>
    {
      _asyncLocalTenant.Value = tenant;
      await action();
      _asyncLocalTenant.Value = null;
    });
  }

  private AsyncLocal<Tenant?> _asyncLocalTenant { get; } = new();

  private ThreadLocal<Tenant?> _threadLocalTenant { get; } = new();
}
