using Xunit.DependencyInjection;
using Xunit.Abstractions;
using Mess.Xunit.Extensions.Xunit;
using Mess.Tenants;

namespace Mess.Xunit.Tenants;

public class TestTenants : ITenants
{
  public Tenant Current =>
    new(
      $"{Test.GetTestIdentifier()} Test",
      "Server=localhost;Port=5432;User Id=mess;Password=mess;Database=mess",
      $"{Test.GetTestIdentifier()}_test"
    );

  public IReadOnlyList<Tenant> All => new List<Tenant> { Current };

  public void Impersonate(Tenant tenant, Action action)
  {
    action();
  }

  public async Task ImpersonateAsync(Tenant tenant, Func<Task> action)
  {
    await action();
  }

  public TestTenants(ITestOutputHelperAccessor testAccessor)
  {
    _testAccessor = testAccessor;
  }

  private readonly ITestOutputHelperAccessor _testAccessor;
  private ITest Test
  {
    get =>
      _testAccessor.Output.GetTest()
      ?? throw new InvalidOperationException("Test is null");
  }
}
