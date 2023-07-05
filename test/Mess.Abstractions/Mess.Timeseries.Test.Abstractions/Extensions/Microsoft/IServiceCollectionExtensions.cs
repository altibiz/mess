using Mess.Relational.Test.Abstractions.Extensions.Microsoft;
using Mess.Timeseries.Migrations;
using Mess.Timeseries.Abstractions.Connection;
using Mess.Timeseries.Connection;
using Mess.Relational.Abstractions.Migrations;

namespace Mess.Timeseries.Test.Abstractions;

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
