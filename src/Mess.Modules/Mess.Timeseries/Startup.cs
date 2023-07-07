using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.Timeseries.Migrations;
using Mess.Timeseries.Abstractions.Connection;
using Mess.Timeseries.Connection;
using Mess.Relational.Abstractions.Migrations;

namespace Mess.Timeseries;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IRelationalDbMigrator, TimeseriesDbMigrator>();
    services.AddScoped<ITimeseriesDbConnection, TimeseriesDbConnection>();
  }
}
