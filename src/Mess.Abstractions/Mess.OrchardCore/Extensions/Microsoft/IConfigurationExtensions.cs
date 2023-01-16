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
          tenant.GetValue<string>("DatabaseConnectionString")
          ?? throw new InvalidOperationException(
            "Tenant is missing a DatabaseConnectionString"
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
