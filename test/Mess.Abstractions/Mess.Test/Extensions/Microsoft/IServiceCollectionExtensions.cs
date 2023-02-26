using Mess.Tenants;
using Mess.Test.E2e;
using Mess.Test.Tenants;

namespace Mess.Test.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void RegisterTestMigrator<T>(this IServiceCollection services)
    where T : class, ITestTenantMigrator
  {
    services.AddScoped<ITestTenantMigrator, T>();
  }

  public static void RegisterTenantFixture(this IServiceCollection services)
  {
    services.AddScoped<ITenants, TestTenants>();
    services.AddScoped<ITenantFixture, TenantFixture>();
  }

  public static void RegisterE2eFixture(this IServiceCollection services)
  {
    services.AddScoped<IE2eFixture, E2eFixture>();
  }
}
