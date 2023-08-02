using System.Reflection;
using Mess.Relational.Abstractions.Migrations;
using Mess.System.Extensions.Microsoft;
using Mess.Timeseries.Abstractions.Context;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using OrchardCore.Environment.Shell;

namespace Mess.Timeseries.Migrations;

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
        foreach (var property in entityType.GetProperties())
        {
          if (
            property.PropertyInfo?.GetCustomAttribute(
              typeof(HypertableColumnAttribute)
            ) != null
          )
          {
            var tableName = entityType.GetTableName();
            // TODO: use non deprecated version
#pragma warning disable CS0618
            var columnName = property.GetColumnName();
#pragma warning restore CS0618
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
