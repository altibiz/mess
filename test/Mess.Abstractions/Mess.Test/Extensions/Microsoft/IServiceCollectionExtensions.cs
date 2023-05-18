using Mess.System.Extensions.Objects;
using Mess.Tenants;
using System.Diagnostics;

namespace Mess.Test.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddTestMigrator<T>(this IServiceCollection services)
    where T : class, ITestTenantMigrator
  {
    services.AddScoped<ITestTenantMigrator, T>();
  }

  public static void AddTenantFixture(this IServiceCollection services)
  {
    services.AddScoped<ITenants, TestTenants>();
    services.AddScoped<ITenantFixture, TenantFixture>();
  }

  public static void AddE2eFixture(this IServiceCollection services)
  {
    services.AddSingleton<E2eTestServer>(
      services => new("http://localhost:5000")
    );

    services.AddScoped<IE2eFixture, E2eFixture>(services =>
    {
      var e2eTestServer = services.GetRequiredService<E2eTestServer>();
      return new("http://localhost:5000", e2eTestServer);
    });
  }

  public static void AddSnapshotFixture(
    this IServiceCollection services,
    Func<object?[]?, string> makeParameterHash
  )
  {
    services.AddScoped<ISnapshotFixture, SnapshotFixture>(
      services => new(services, makeParameterHash)
    );
  }

  public static void AddSnapshotFixture(this IServiceCollection services)
  {
    services.AddSnapshotFixture(
      parameters =>
        parameters is null or { Length: 0 }
          ? "empty-parameters"
          : parameters
            .Select(
              argument =>
                new
                {
                  Type = argument?.GetType()?.FullName,
                  Argument = argument
                }
            )
            .GetJsonMurMurHash()
    );
  }
}
