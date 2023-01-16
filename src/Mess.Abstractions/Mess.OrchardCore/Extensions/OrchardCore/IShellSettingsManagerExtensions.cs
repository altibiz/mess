using Mess.Tenants;
using OrchardCore.Environment.Shell;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class IShellSettingsManagerExtensions
{
  public static IReadOnlyDictionary<
    string,
    IReadOnlyList<Tenant>
  > GetTenantsGroupedByConnectionString(
    this IShellSettingsManager shellSettingsManager
  ) =>
    shellSettingsManager
      .LoadSettingsAsync()
      .Result.GroupBy(settings => settings["ConnectionString"])
      .ToDictionary(
        group => group.Key,
        group =>
          group.Select(ShellSettingsExtensions.GetTenant).ToList()
          as IReadOnlyList<Tenant>
      );
}
