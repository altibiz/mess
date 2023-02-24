using Mess.Timeseries.Test.Abstractions.Tenants;
using Mess.Xunit.Tenants;

namespace Mess.Timeseries.Test.Abstractions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection RegisterTimeseriesTestMigrator(
    this IServiceCollection services
  )
  {
    services.AddScoped<ITestMigrator, TimeseriesTestMigrator>();
    return services;
  }
}
