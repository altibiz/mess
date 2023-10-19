using System.Diagnostics;
using System.Reflection;
using Mess.Relational.Abstractions.Migrations;
using Mess.System.Extensions.Microsoft;
using Mess.Timeseries.Abstractions.Context;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using OrchardCore.Environment.Shell;

namespace Mess.Timeseries;

public class TimeseriesDbMigrator : IRelationalDbMigrator
{
  public async Task MigrateAsync()
  {
    var contexts =
      _serviceProvider.GetServicesInheriting<TimeseriesDbContext>();

    foreach (var context in contexts)
    {
      await context.Database.MigrateAsync();
      _logger.LogDebug(
        "Timeseries database {0} migrated for tenant {1}",
        context.GetType().Name,
        _shellSettings.Name
      );

      var entityTypes = context.Model.GetEntityTypes();
      foreach (var entityType in entityTypes)
      {
        var tableName = entityType.GetTableName();
        if (tableName == null)
        {
          continue;
        }

        foreach (var property in entityType.GetProperties())
        {
          if (
            property.PropertyInfo?.GetCustomAttribute(
              typeof(HypertableColumnAttribute)
            )
            is null
          )
          {
            continue;
          }

          var columnName = property.GetColumnName();
          await CreateHypertableAsync(context, tableName, columnName);

          _logger.LogDebug(
            "Created hypertable {} for column {} on {} for {}",
            tableName,
            columnName,
            context.GetType().Name,
            _shellSettings.Name
          );
        }
      }
    }
  }

  // TODO: make this unstoppable by the debugger
  // [DebuggerHidden]
  private static async Task CreateHypertableAsync(
    TimeseriesDbContext context,
    string tableName,
    string columnName
  )
  {
    try
    {
      await context.Database.ExecuteSqlRawAsync(
        $"SELECT create_hypertable('\"{tableName}\"', '{columnName}');"
      );
    }
    catch (PostgresException exception)
    {
      // NOTE: hypertable already exists
      if (exception.SqlState != "TS110")
      {
        throw exception;
      }
    }
  }

  public TimeseriesDbMigrator(
    ILogger<TimeseriesDbMigrator> logger,
    IServiceProvider serviceProvider,
    ShellSettings shellSettings
  )
  {
    _logger = logger;
    _serviceProvider = serviceProvider;
    _shellSettings = shellSettings;
  }

  private readonly ILogger _logger;
  private readonly ShellSettings _shellSettings;
  private readonly IServiceProvider _serviceProvider;
}
