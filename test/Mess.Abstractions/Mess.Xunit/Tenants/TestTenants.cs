using Xunit.DependencyInjection;
using Xunit.Abstractions;
using Mess.Xunit.Extensions.Xunit;
using Mess.Xunit.Extensions.Npgsql;
using Mess.System.Extensions.String;
using Mess.Tenants;

namespace Mess.Xunit.Tenants;

public class TestTenants : ITenants
{
  public Tenant Current => new(GetTenantName(), GetTenantConnectionString());

  // NOTE: not needed for tests
  public IReadOnlyList<Tenant> All => throw new NotImplementedException();

  // NOTE: not needed for tests
  public void Impersonate(Tenant tenant, Action action)
  {
    throw new NotImplementedException();
  }

  // NOTE: not needed for tests
  public Task ImpersonateAsync(Tenant tenant, Func<Task> action)
  {
    throw new NotImplementedException();
  }

  public string GetTenantName()
  {
    var tenantName =
      Tenant.GetValue<string>("Name")
      ?? throw new InvalidOperationException($"Tenant must have a Name");
    return $"{tenantName}-{Test.GetTestIdentifier()}";
  }

  public string GetTenantConnectionString() =>
    (
      Tenant.GetValue<string>("ConnectionString")
      ?? throw new InvalidOperationException(
        $"Tenant must have a ConnectionString"
      )
    ).RegexReplace(
      ConnectionStringExtensions.CONNECTION_STRING_DATABASE_REGEX,
      $"Database=$1.{Test.GetTestIdentifier()}"
    );

  public IConfigurationSection Tenant
  {
    get =>
      Configuration
        .GetRequiredSection("Mess")
        .GetRequiredSection("Test")
        .GetRequiredSection("Tenant");
  }

  public TestTenants(
    IConfiguration configuration,
    ITestOutputHelperAccessor testAccessor
  )
  {
    Configuration = configuration;
    TestAccessor = testAccessor;
  }

  private IConfiguration Configuration { get; }
  private ITestOutputHelperAccessor TestAccessor { get; }
  private ITest Test
  {
    get =>
      TestAccessor.Output.GetTest()
      ?? throw new InvalidOperationException("Test is null");
  }
}
