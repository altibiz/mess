using Mess.Prelude.Extensions.Microsoft;
using Mess.Relational.Abstractions.Context;
using Mess.Relational.Abstractions.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace Mess.Relational;

public class RelationalDbMigrator : IRelationalDbMigrator
{
  public async Task MigrateAsync()
  {
    var contexts =
      _serviceProvider.GetServicesInheriting<RelationalDbContext>();

    foreach (var context in contexts)
    {
      await context.Database.MigrateAsync();

      _logger.LogDebug(
        "Relational database {} migrated for tenant {}",
        context.GetType().Name,
        _shellSettings.Name
      );
    }
  }

  public RelationalDbMigrator(
    ILogger<RelationalDbMigrator> logger,
    ShellSettings shellSettings,
    IServiceProvider serviceProvider
  )
  {
    _logger = logger;
    _shellSettings = shellSettings;
    _serviceProvider = serviceProvider;
  }

  private readonly ILogger _logger;
  private readonly ShellSettings _shellSettings;
  private readonly IServiceProvider _serviceProvider;
}
