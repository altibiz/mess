using Mess.System.Extensions.Objects;
using Mess.Tenants;

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

  public static void RegisterE2eFixture(
    this IServiceCollection services,
    string baseUrl,
    Func<CancellationToken, Task> makeAppTask
  )
  {
    services.AddScoped<IE2eFixture, E2eFixture>(
      services => new(baseUrl, makeAppTask)
    );
  }

  public static void RegisterE2eFixture(this IServiceCollection services)
  {
    // TODO: actually spawn something
    services.RegisterE2eFixture(
      "",
      token =>
      {
        return Task.CompletedTask;
      }
    );
  }

  public static void RegisterSnapshotFixture(
    this IServiceCollection services,
    Func<object?[]?, string> makeParameterHash
  )
  {
    services.AddScoped<ISnapshotFixture, SnapshotFixture>(
      services => new(services, makeParameterHash)
    );
  }

  public static void RegisterSnapshotFixture(this IServiceCollection services)
  {
    services.RegisterSnapshotFixture(
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
