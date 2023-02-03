using Mess.Tenants;
using Mess.Xunit.Tenants;

namespace Mess.Xunit.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void RegisterTestMigrator<T>(this IServiceCollection services)
    where T : class, ITestMigrator
  {
    services.AddScoped<ITestMigrator, T>();
  }

  public static void RegisterTestTenants(this IServiceCollection services)
  {
    services.AddScoped<ITenants, TestTenants>();
  }
}
