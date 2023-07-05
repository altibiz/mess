using Mess.OrchardCore.Extensions.Objects;
using Mess.System.Test.Extensions.Microsoft;

namespace Mess.OrchardCore.Test.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddOrchardSnapshotFixture(this IServiceCollection services)
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
            .GetNewtonsoftJsonMurMurHash()
    );
  }
}
