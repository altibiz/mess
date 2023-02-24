using Mess.Tenants;

namespace Mess.Timeseries.Abstractions.Client;

public record class DesignTimeTenants(
  string TenantName,
  string ConnactionString,
  string TablePrefix
) : ITenants
{
  public Tenant Current => new(TenantName, ConnactionString, TablePrefix);

  // NOTE: not needed for design
  public IReadOnlyList<Tenant> All => throw new NotImplementedException();

  // NOTE: not needed for design
  public void Impersonate(Tenant tenant, Action action)
  {
    throw new NotImplementedException();
  }

  // NOTE: not needed for design
  public Task ImpersonateAsync(Tenant tenant, Func<Task> action)
  {
    throw new NotImplementedException();
  }
}
