namespace Mess.OrchardCore.Extensions.Microsoft;

public static class IConfigurationExtensions
{
  public static IDictionary<
    string,
    IEnumerable<string>
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
          tenant.GetValue<string>("ConnectionString")
          ?? throw new InvalidOperationException(
            "Tenant is missing a ConnectionString"
          )
      )
      .ToDictionary(
        group => group.Key,
        group =>
          group.Select(
            tenant =>
              tenant.GetValue<string>("Name")
              ?? throw new InvalidOperationException(
                "Tenant is missing a ShellName"
              )
          )
      );
}
