using Mess.Tenants;

namespace Mess.Test.Tenants;

public interface ITenantFixture
{
  public ITenants Tenants { get; }
}
