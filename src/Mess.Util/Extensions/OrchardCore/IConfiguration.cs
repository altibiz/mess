using Microsoft.Extensions.Configuration;

namespace Mess.Util.Extensions.OrchardCore;

public static class IConfigurationExtensions
{
  public static IDictionary<
    string,
    IEnumerable<string>
  > GetAutoSetupTenantNamesGroupedByConnectionString(
    this IConfiguration configuration
  ) =>
    configuration
      .GetRequiredSection("OrchardCore")
      .GetRequiredSection("OrchardCore_AutoSetup")
      .GetRequiredSection("Tenants")
      .GetChildren()
      .GroupBy(
        tenant =>
          tenant.GetValue<string>("DatabaseConnectionString")
          ?? throw new InvalidOperationException(
            "Tenant is missing a ConnectionString"
          )
      )
      .ToDictionary(
        group => group.Key,
        group =>
          group.Select(
            tenant =>
              tenant.GetValue<string>("ShellName")
              ?? throw new InvalidOperationException(
                "Tenant is missing a ShellName"
              )
          )
      );
}
