using Mess.Relational.Abstractions.Migrations;
using Mess.Relational.Migrations;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Relational;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IRelationalDbMigrator, RelationalDbMigrator>();
    services.AddScoped<
      IModularTenantEvents,
      RelationalDbMigrationModularTenantEvents
    >();
  }
}
