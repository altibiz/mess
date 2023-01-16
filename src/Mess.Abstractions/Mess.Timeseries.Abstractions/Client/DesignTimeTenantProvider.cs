using Mess.Tenants;

namespace Mess.Timeseries.Abstractions.Client;

public record class DesignTimeTenantProvider(
  string tenantName,
  string connactionString
) : ITenantProvider
{
  public string GetTenantName() => tenantName;

  public string GetTenantConnectionString() => connactionString;
}
