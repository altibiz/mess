namespace Mess.Tenants;

public interface ITenantProvider
{
  public string GetTenantName();

  public string GetTenantConnectionString();
}
