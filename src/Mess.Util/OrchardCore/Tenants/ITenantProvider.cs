namespace Mess.Util.OrchardCore.Tenants;

public interface ITenantProvider
{
  public string GetTenantName();

  public string GetTenantConnectionString();
}
