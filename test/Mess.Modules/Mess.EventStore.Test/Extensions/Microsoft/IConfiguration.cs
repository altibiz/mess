namespace Mess.EventStore.Test.Extensions.Microsoft;

public static class IConfigurationExtensions
{
  public static IDictionary<
    string,
    IEnumerable<string>
  > GetEventStoreTestTenantNamesGroupedByConnectionString(
    this IConfiguration configuration
  ) =>
    configuration
      .GetRequiredSection("Mess")
      .GetRequiredSection("EventStore")
      .GetRequiredSection("Test")
      .GetRequiredSection("Tenant") switch
    {
      IConfigurationSection tenant
        => new Dictionary<string, IEnumerable<string>>
        {
          {
            tenant.GetValue<string>("ConnectionString")
              ?? throw new InvalidOperationException(
                "Tenant must have a ConnectionString"
              ),
            new[]
            {
              tenant.GetValue<string>("Name")
                ?? throw new InvalidOperationException(
                  "Tenant must have a Name"
                ),
            }
          }
        },
      null => throw new InvalidOperationException("Tenant must be defined")
    };
}
