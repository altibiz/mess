using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Client;
using Mess.Timeseries.Test.Abstractions.Tenants;
using Mess.Test.Tenants;

namespace Mess.Timeseries.Test.Abstractions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection RegisterTestTimeseriesStore(
    this IServiceCollection services
  )
  {
    services.AddScoped<ITimeseriesMigrator, TimeseriesMigrator>();
    services.AddScoped<ITestTenantMigrator, TimeseriesTestTenantMigrator>();
    return services;
  }
}
