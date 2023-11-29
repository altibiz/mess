using Mess.OrchardCore.Extensions.Objects;
using Mess.System.Test.Extensions.Microsoft;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;

namespace Mess.OrchardCore.Test.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void AddOrchardCoreShellSettings(
    this IServiceCollection services
  ) =>
    services.AddTransient(services =>
    {
      var testId = services.GetTestId();

      return new ShellSettings(
        new ShellConfiguration(
          new ConfigurationBuilder()
            .AddInMemoryCollection(
              new Dictionary<string, string?> { { "RequestUrlPrefix", testId } }
            )
            .Build()
        ),
        new ShellConfiguration(
          new ConfigurationBuilder()
            .AddInMemoryCollection(
              new Dictionary<string, string?>
              {
                {
                  "ConnectionString",
                  "Server=localhost;Port=5432;User Id=mess;Password=mess;Database=mess"
                },
                { "TablePrefix", testId }
              }
            )
            .Build()
        )
      )
      {
        Name = testId,
      };
    });

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
