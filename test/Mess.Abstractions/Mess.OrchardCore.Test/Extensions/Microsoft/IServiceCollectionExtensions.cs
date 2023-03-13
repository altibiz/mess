using Mess.OrchardCore.Test.Snapshots;

namespace Mess.OrchardCore.Test.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static void RegisterOrchardSnapshotFixture(
    this IServiceCollection services
  )
  {
    services.AddScoped<ISnapshotFixture, OrchardSnapshotFixture>();
  }
}
