using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Scope;

namespace Mess.Cms.Extensions.OrchardCore;

public static class ShellScopeExtensions
{
  //
  // Summary:
  //     Execute a delegate using a child scope created from the current one.
  public static async Task UsingChildScopeAsync<T>(
    Func<ShellScope, Task<T>> execute,
    bool activateShell = true
  )
  {
    await (await ShellScope.CreateChildScopeAsync()).UsingAsync(
      execute,
      activateShell
    );
  }

  //
  // Summary:
  //     Execute a delegate using a child scope created from the current one.
  public static async Task UsingChildScopeAsync(
    ShellSettings settings,
    Func<ShellScope, Task> execute,
    bool activateShell = true
  )
  {
    await (await ShellScope.CreateChildScopeAsync(settings)).UsingAsync(
      execute,
      activateShell
    );
  }

  //
  // Summary:
  //     Execute a delegate using a child scope created from the current one.
  public static async Task UsingChildScopeAsync(
    string tenant,
    Func<ShellScope, Task> execute,
    bool activateShell = true
  )
  {
    await (await ShellScope.CreateChildScopeAsync(tenant)).UsingAsync(
      execute,
      activateShell
    );
  }
}
