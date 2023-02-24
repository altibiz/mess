using Mess.Tenants;

namespace Mess.OrchardCore.Extensions.Microsoft;

public static class IConfigurationExtensions
{
  public static IReadOnlyDictionary<
    string,
    IReadOnlyList<Tenant>
  > GetOrchardCoreAutoSetupTenantNamesGroupedByConnectionString(
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
          group.Select(tenant => tenant.GetTenant()).ToList()
          as IReadOnlyList<Tenant>
      );
}
