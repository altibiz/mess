using Mess.System.Test.Extensions.Microsoft;
using Mess.Relational.Abstractions.Migrations;
using Mess.Relational.Migrations;
using Mess.Relational.Test.Abstractions.Migrations;

namespace Mess.Relational.Test.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddTestRelationalStore(
    this IServiceCollection services
  )
  {
    services.AddScoped<IRelationalDbMigrator, RelationalDbMigrator>();
    services.AddTestMigrator<RelationalDbTestMigrator>();
    return services;
  }
}
