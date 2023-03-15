using Mess.OrchardCore.Extensions.Objects;
using Mess.OrchardCore.Test.Snapshots;
using Mess.Test.Extensions.Microsoft;

namespace Mess.OrchardCore.Test.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void RegisterOrchardSnapshotFixture(
    this IServiceCollection services
  )
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
            .GetNewtonsoftJsonMurMurHash()
    );
  }

  public static void RegisterOrchardE2eFixture(this IServiceCollection services)
  {
    // TODO: actually spawn orchard core
    services.RegisterE2eFixture(
      "",
      token =>
      {
        return Task.CompletedTask;
      }
    );
  }
}
