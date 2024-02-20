using Mess.Cms.Extensions.Microsoft;
using Mess.Relational.Abstractions.Migrations;
using Mess.Timeseries.Abstractions.Connection;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Timeseries;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IRelationalDbMigrator, TimeseriesDbMigrator>();
    services.AddScoped<ITimeseriesDbConnection, TimeseriesDbConnection>();

    services.AddBackgroundTask<MaterializedViewRefreshBackgroundTask>();
  }
}
