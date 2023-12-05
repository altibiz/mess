using Mess.Relational.Abstractions.Migrations;
using Mess.Relational.Test.Abstractions.Extensions;
using Mess.Timeseries.Abstractions.Connection;

namespace Mess.Timeseries.Test.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddTestTimeseriesStore(
    this IServiceCollection services
  )
  {
    services.AddTestRelationalStore();
    services.AddScoped<IRelationalDbMigrator, TimeseriesDbMigrator>();
    services.AddScoped<ITimeseriesDbConnection, TimeseriesDbConnection>();
    return services;
  }
}
